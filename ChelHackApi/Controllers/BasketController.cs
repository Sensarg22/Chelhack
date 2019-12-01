using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ChelHackApi.Controllers
{
    [Route("[controller]")]
    public class BasketController : Controller
    {
        private readonly IMongoDatabase _mongoDatabase;
        private IMongoCollection<BasketModel> _basketCollection;

        public BasketController(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
            _basketCollection = mongoDatabase.GetCollection<BasketModel>("Basket");
        }
        
        [HttpPost("submit")]
        public async Task<IActionResult> Submit(BasketModel model)
        {
            model.Id = ObjectId.GenerateNewId();
            model.Added = DateTime.UtcNow;
            await _basketCollection.InsertOneAsync(model);
            return Ok("Success!");
        }
    }

    public class BasketModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public IEnumerable<BasketItem> Items { get; set; }
        public DateTime Added { get; set; } 
    }

    public class BasketItem
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
    }
}