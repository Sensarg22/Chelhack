using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ChelHackApi.Controllers
{
    [Route("[controller]")]
    public class CategoriesController : Controller
    {
        [HttpGet("")]
        public async Task<List<string>> Categories()
        {
            return new List<string>
            {
                "Мобильные телефоны",
                "Ноутбуки",
                "Кружки",
                "Тапки"
            };
        }
    }
}