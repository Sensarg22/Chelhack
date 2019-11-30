using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChelHackApi.Controllers
{
    [Route("[controller]")]
    public class GoodsController : Controller
    {
        [HttpGet("")]
        public async Task<List<Good>> Goods([FromQuery] PagedFilter filter)
        {
            var goods = TestGoods.List;
            if (!string.IsNullOrEmpty(filter.TextFilter))
            {
                goods = goods.Where(x => x.Title == filter.TextFilter).ToList();
            }
            
            return goods.Skip((filter.Page == 1 ? 0 : filter.Page) * filter.PageSize)
                .Take(filter.PageSize).ToList();
        }
        
        [HttpGet("{id:long:min(1)}")]
        public async Task<Good> Good(long id)
        {
            return TestGoods.List.FirstOrDefault(x => x.Id == id);
        }
    }

    public class PagedFilter
    {

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string TextFilter { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
    }
}