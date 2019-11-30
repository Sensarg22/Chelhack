using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ChelHackApi.Controllers
{
    [Route("[controller]")]
    public class BrandsController : Controller
    {
        [HttpGet("")]
        public async Task<List<string>> Brands()
        {
            return new List<string>
            {
                "Apple",
                "Google",
                "Xuavei",
                "Sony",
                "Microsoft"
            };
        }
    }
}