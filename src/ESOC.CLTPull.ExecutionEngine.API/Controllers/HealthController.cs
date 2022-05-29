using ESOC.CLTPull.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESOC.CLTPull.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var response = new HealthResponse()
            {
                Name = "ESOC.CLTPull.WebApi.Controllers.WebApi",
                Status = "All Good"
            };
            return new OkObjectResult(response);

        }

    }
}
