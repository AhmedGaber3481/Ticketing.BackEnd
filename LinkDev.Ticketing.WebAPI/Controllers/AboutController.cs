using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Ticketing.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        [HttpGet]
        public ActionResult About(int pageId, int pageSize)
        {
            return Ok(new { Version = 1});
        }
    }
}
