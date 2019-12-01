using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ChelHackApi.Controllers
{
    [Route("[controller]")]
    public class GoodsController : Controller
    {
        private readonly IMongoDatabase _mongoDatabase;
        private IMongoCollection<BsonDocument> _goodsCollection;

        public GoodsController(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
            _goodsCollection = _mongoDatabase.GetCollection<BsonDocument>(nameof(Good));
        }
        
        [HttpGet("")]
        public async Task<List<Good>> Goods([FromQuery] PagedFilter filter)
        {
            var filterDefinition = FilterDefinition<BsonDocument>.Empty;
            var sortDefinition = Builders<BsonDocument>.Sort.Ascending(filter.SortField ?? "Price");
            
            if (!string.IsNullOrEmpty(filter.TextFilter))
            {
                filterDefinition = Builders<BsonDocument>.Filter.Text(filter.TextFilter);
            }

            if (!string.IsNullOrEmpty(filter.Brand))
            {
                filterDefinition &= Builders<BsonDocument>.Filter.Eq(x => x["Brand"],filter.Brand);
            }
            
            if (!string.IsNullOrEmpty(filter.Category))
            {
                filterDefinition &= Builders<BsonDocument>.Filter.Eq(x => x["Category"],filter.Category);
            }

            if (!string.IsNullOrEmpty(filter.SortField))
            {
                if (filter.SortOrder == "desc")
                {
                    sortDefinition = Builders<BsonDocument>.Sort.Descending(filter.SortField);
                }
            }
            
            return await _goodsCollection.Find(filterDefinition)
                .Sort(sortDefinition)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Limit(filter.PageSize)
                .Project(x => BsonSerializer.Deserialize<Good>(x, null))
                .ToListAsync();
        }
        
        [HttpGet("{id:long:min(1)}")]
        public async Task<Good> Good(long id)
        {
            return await _goodsCollection.Find(Builders<BsonDocument>.Filter.Eq(x => x["_id"], id))
                .Project(x => BsonSerializer.Deserialize<Good>(x, null)).FirstOrDefaultAsync();
        }
    }
}