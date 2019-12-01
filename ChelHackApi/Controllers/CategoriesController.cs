using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ChelHackApi.Controllers
{
    [Route("[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IMongoDatabase _mongoDatabase;
        private IMongoCollection<Category> _categoriesCollection;

        public CategoriesController(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
            _categoriesCollection = _mongoDatabase.GetCollection<Category>(nameof(Category));
        }
        
        [HttpGet("")]
        public async Task<List<string>> Categories()
        {
            return await _categoriesCollection
                .Find(Builders<Category>.Filter.Empty)
                .Project(x => x.Name)
                .ToListAsync();
        }
    }
}