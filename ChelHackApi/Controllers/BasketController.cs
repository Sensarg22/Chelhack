using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ChelHackApi.Controllers
{
    [Route("[controller]")]
    public class BasketController : Controller
    {
        [HttpGet("submit")]
        public async Task<IActionResult> Submit()
        {
            return Ok("Success!");
        }
    }
}