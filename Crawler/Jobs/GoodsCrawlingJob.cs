using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common;
using Domain;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Quartz;

namespace Crawler.Jobs
{
    [DisallowConcurrentExecution]
    public class GoodsCrawlingJob : IJob
    {
        private readonly IMongoDatabase _database;
        private readonly IConfiguration _configuration;
        private readonly JsonStreamHttpClient _streamHttpClient;
        private readonly IMongoCollection<Good> _goodsCollection;
        private readonly IMongoCollection<Brand> _brandsCollection;
        
        private const int ChunkSize = 5;

        public GoodsCrawlingJob(IMongoDatabase database, IConfiguration configuration, 
            JsonStreamHttpClient streamHttpClient)
        {
            _database = database;
            _configuration = configuration;
            _streamHttpClient = streamHttpClient;
            _goodsCollection = database.GetCollection<Good>(nameof(Good));
            _brandsCollection = database.GetCollection<Brand>(nameof(Brand));
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var goodsResult = await _streamHttpClient.GetFromJsonStreamAsync<RawGoodResult>(_configuration[Constants.GoodsSourceGoods]);
                var goods = goodsResult.Data.Select(ToGood).ToList();

                var actualIds = goods.Select(x => x.Id).ToList();
                var existingGoods = _goodsCollection
                    .Find(Builders<Good>.Filter.In(x => x.Id, actualIds))
                    .Project(x => new {x.Id, x.Hash})
                    .ToList();

                var toInsert = new List<Good>();
                var toUpdate = new List<Good>();
                
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

                await UpdateBrands(goods);
                
                await DeleteNonExistingGoods(actualIds);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static Good ToGood(RawGoodModel rawGoodModel)
        {
            return new Good
            {
                Id = rawGoodModel.Id,
                Availability = rawGoodModel.Availability,
                Brand = rawGoodModel.Brand,
                Category = rawGoodModel.Category,
                Price = rawGoodModel.Price,
                Quantity = rawGoodModel.Quantity,
                Title = rawGoodModel.Title,
                BrandId = rawGoodModel.BrandId,
                FinalPrice = rawGoodModel.FinalPrice,
                ImageUrl = rawGoodModel.ImageUrl,
                AvailableInDays = rawGoodModel.AvailableInDays,
                Parameters = rawGoodModel.Parameters.Select(y => new Parameter
                {
                    Title = y.Title,
                    Value = y.Value
                }).ToList()
            };
        }

        private async Task DownloadPictures(IList<string> urls)
        {
            if (!Directory.Exists(_configuration[Constants.ImagesDirectory]))
            {
                Directory.CreateDirectory(_configuration[Constants.ImagesDirectory]);
            }
            
            Parallel.ForEach(urls, (url) =>
            {
                var absoluteUrl = new Uri(_configuration[Constants.GoodsSourceBase] + url);
                var fName = Path.GetFileName(url);
                var fullFileName = Path.Combine(_configuration[Constants.ImagesDirectory], fName);
                if (File.Exists(fullFileName))
                {
                    File.Delete(fullFileName);
                }

                try
                {
                    Console.WriteLine($"DOWNLOADING {url}");
                    new WebClient().DownloadFile(absoluteUrl, fullFileName);
                    Console.WriteLine($"DOWNLOADED {url}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        private async Task DeleteNonExistingGoods(List<long> actualItemIds)
        {
            await _goodsCollection.DeleteManyAsync(Builders<Good>.Filter.Nin(x => x.Id, actualItemIds));
        }

        private async Task InsertGoodsAsync(IReadOnlyCollection<Good> items)
        {
            await _goodsCollection.InsertManyAsync(items);
        }

        private async Task UpdateBrands(IReadOnlyList<Good> items)
        {
            var writeOptions = new BulkWriteOptions
            {
                IsOrdered = true
            };
            var brandOperations = new List<ReplaceOneModel<Brand>>();
            brandOperations.AddRange(items.Select(x => new ReplaceOneModel<Brand>(Builders<Brand>.Filter.Eq(y => y.Name, x.Brand),
                new Brand
                {
                    Id = x.BrandId,
                    Name = x.Brand
                })
            {
                IsUpsert = true
            }));
            if (brandOperations.Any())
            {
                await _brandsCollection.BulkWriteAsync(brandOperations, writeOptions);
            }
        }

        private async Task UpdateGoodsAsync(IReadOnlyList<Good> items)
        {
            var writeOptions = new BulkWriteOptions
            {
                IsOrdered = true
            };
            var models = items.Select(x => new ReplaceOneModel<BsonDocument>(Builders<BsonDocument>.Filter.Eq("_id", x.Id), x.ToBsonDocument())
            {
                IsUpsert = true
            });

            await _database.GetCollection<BsonDocument>(nameof(Good)).BulkWriteAsync(models, writeOptions);
        }

        private static void CalculateHash(Good good)
        {
            good.Hash = GoodHasher.Calculate(good);
        }
    }
}