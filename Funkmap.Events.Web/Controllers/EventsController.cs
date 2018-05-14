using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funkmap.Events.Web.Controllers
{
    [Route("api/event")]
    public class EventsController : Controller
    {
        [HttpPost]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> CreateEvent()
        {
            return Ok("success");
        }
    }
}