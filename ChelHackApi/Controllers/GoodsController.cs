using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ChelHackApi.Controllers
{
    [Route("[controller]")]
    public class GoodsController : Controller
    {
        private readonly IMongoDatabase _mongoDatabase;

        public GoodsController(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }
        
        [HttpGet("")]
        public async Task<List<Good>> Goods([FromQuery] PagedFilter filter)
        {
            var collection = _mongoDatabase.GetCollection<Good>(nameof(Good));
            var filterDefinition = FilterDefinition<Good>.Empty;
            
            if (!string.IsNullOrEmpty(filter.TextFilter))
            {
                filterDefinition = Builders<Good>.Filter.Text(filter.TextFilter);
            }
            
            return await collection.Find(filterDefinition)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Limit(filter.PageSize).ToListAsync();
        }
        
        [HttpGet("{id:long:min(1)}")]
        public async Task<Good> Good(long id)
        {
            return TestGoods.List.FirstOrDefault(x => x.Id == id);
        }
    }
}