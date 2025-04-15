using EkbCulture.ServiceDefaults.Models;
using Microsoft.AspNetCore.Mvc;

namespace EkbCulture.Controllers
{
    [ApiController]
    [Route("api/locations")]
    public class LocationController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetLocations()
        {
            // Логика получения локаций
            return Ok(new List<Location>());
        }
    }
}