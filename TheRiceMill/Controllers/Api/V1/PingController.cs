using Microsoft.AspNetCore.Mvc;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    public class PingController : BaseController
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}