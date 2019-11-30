using System.Collections.Generic;

namespace Domain.Entities
{
    public class Good
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Availability { get; set; }
        public int Price { get; set; }
        public int FinalPrice { get; set; }
        public int AvailableInDays { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string Hash { get; set; }
        
        
        public List<Parameter> Parameters { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
    }
}