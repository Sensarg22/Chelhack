using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quartz;

namespace Crawler.Jobs
{
    [DisallowConcurrentExecution]
    public class GoodsCrawlingJob : IJob
    {
        private readonly IMongoDatabase _database;
        private readonly HttpClient _httpClient;
        private const int chunkSize = 5;
        private readonly IConfiguration _configuration;

        public GoodsCrawlingJob(IHttpClientFactory httpClientFactory, IMongoDatabase database,
            IConfiguration configuration)
        {
            _database = database;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient(Constants.GoodsHttpClientName);
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var result = await _httpClient.GetAsync(_configuration[Constants.GoodsSourceGoods]);
                result.EnsureSuccessStatusCode();
                var rawResult = await result.Content.ReadAsStringAsync();
                var goodsResult = JsonConvert.DeserializeObject<RawGoodResult>(rawResult, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                var goods = goodsResult.Data.Select(x => new Good
                {
                    Id = x.Id,
                    Availability = x.Availability,
                    Brand = x.Brand,
                    Category = x.Category,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Title = x.Title,
                    BrandId = x.BrandId,
                    FinalPrice = x.FinalPrice,
                    ImageUrl = x.ImageUrl,
                    AvailableInDays = x.AvailableInDays,
                    Parameters = x.Parameters.Select(y => new Parameter
                    {
                        Title = y.Title,
                        Value = y.Value
                    }).ToList()
                }).ToList();


                var actualIds = goods.Select(x => x.Id).ToList();
                var existingGoods = _database.GetCollection<Good>(nameof(Good))
                    .AsQueryable()
                    .Where(x => actualIds.Contains(x.Id))
                    .Select(x => new {Id = x.Id, Hash = x.Hash})
                    .ToList();

                var toInsert = new List<Good>();
                var toUpdate = new List<Good>();
                var toDelete = _database.GetCollection<Good>(nameof(Good))
                    .AsQueryable()
                    .Where(x => !actualIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToList();
                
                foreach (var good in goods)
                {
                    CalculateHash(good);
                    var existing = existingGoods.FirstOrDefault(x => x.Id == good.Id);
                    if (existing != null)
                    {
                        if (existing.Hash != good.Hash)
                        {
                            toUpdate.Add(good);
                        }
                    }
                    else
                    {
                        toInsert.Add(good);
                    }
                }

                if (toUpdate.Any())
                {
                    await UpdateGoodsAsync(toUpdate);
                }

                if (toInsert.Any())
                {
                    await InsertGoodsAsync(toInsert);
                    await DownloadPictures(toInsert.Select(x => x.ImageUrl).ToList());
                }

                if (toDelete.Any())
                {
                    await DeleteGoods(toDelete);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task DownloadPictures(IList<string> urls)
        {
            int skip = 0;
            int count;
            do
            {
                var urlsChunk = urls.Skip(skip).Take(chunkSize).ToList();

                if (urlsChunk.Any())
                {
                    var tasks = urlsChunk.Select(x =>
                        {
                            var absoluteUrl = new Uri(_configuration[Constants.GoodsSourceBase]+ x);
                            var fName = Path.GetFileName(x);
                            var fullFileName = Path.Combine(@"/Users/axel/Dev/Chelhack/ChelHackWeb/images/", fName);
                            if (File.Exists(fullFileName))
                            {
                                File.Delete(fullFileName);
                            }
                            return new WebClient().DownloadFileTaskAsync(absoluteUrl, fullFileName);
                        }
                    );
                    
                    try
                    {
                        await Task.WhenAll(tasks);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                
                skip = skip + urlsChunk.Count;
                count = urlsChunk.Count;
            } while (count >= chunkSize);
            
            
            throw new NotImplementedException();
        }

        private async Task DeleteGoods(List<long> toDelete)
        {
            var collection = _database.GetCollection<Good>(nameof(Good));
            await collection.DeleteManyAsync(Builders<Good>.Filter.In(x => x.Id, toDelete));
        }

        private async Task InsertGoodsAsync(IReadOnlyCollection<Good> items)
        {
            var collection = _database.GetCollection<Good>(nameof(Good));
            await collection.InsertManyAsync(items);
        }

        private async Task UpdateGoodsAsync(IReadOnlyList<Good> items)
        {
            var collection = _database.GetCollection<Good>(nameof(Good));

            var models = new WriteModel<Good>[items.Count];
                
            for (var i = 0; i < items.Count; i++){
                models[i] = new ReplaceOneModel<Good>(Builders<Good>.Filter.Eq(x => x.Id, items[i].Id), items[i])
                {
                    IsUpsert = true
                };
            };

            await collection.BulkWriteAsync(models);
        }

        private static void CalculateHash(Good good)
        {
            good.Hash = GoodHasher.Calculate(good);
        }
    }
}