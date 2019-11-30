using System.Collections.Generic;

namespace Crawler.Jobs
{
    public class RawGoodModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Availability { get; set; }
        public int Price { get; set; }
        public int FinalPrice { get; set; }
        public string Category { get; set; }
        public int AvailableInDays { get; set; }
        public string Brand { get; set; }
        public int BrandId { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public List<RawParameter> Parameters { get; set; }
    }
}