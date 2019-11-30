using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class Good
    {
        [BsonId]
        public long Id { get; set; }
        public string Title { get; set; }
        public bool Availability { get; set; }
        public int Price { get; set; }
        public int FinalPrice { get; set; }
        public int AvailableInDays { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public int BrandId { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string Hash { get; set; }
        
        public List<Parameter> Parameters { get; set; }

        public override string ToString()
        {
            return $"{Title}, {Availability}, " +
                   $"{Price}, {FinalPrice}, " +
                   $"{AvailableInDays}, {Quantity}, " +
                   $"{ImageUrl}, {Brand}, " +
                   $"{Category}, {string.Join(",",Parameters.Select(x => x.ToString()))}";
        }
    }
}