using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChelHackApi.Controllers
{
    [Route("[controller]")]
    public class BrandsController : Controller
    {
        private readonly IMongoDatabase _mongoDatabase;
        private IMongoCollection<Brand> _brandsCollection;

        public BrandsController(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
            _brandsCollection = _mongoDatabase.GetCollection<Brand>(nameof(Brand));
        }
        [HttpGet("")]
        public async Task<List<string>> Brands()
        {
            return await _brandsCollection
                .Find(FilterDefinition<Brand>.Empty)
                .Project(x => x.Name)
                .ToListAsync();
        }
    }
}