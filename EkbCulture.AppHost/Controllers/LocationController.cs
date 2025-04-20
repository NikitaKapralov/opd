using EkbCulture.AppHost.Models;
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
        [HttpGet]
        public IActionResult GetLocation(string id)
        {
            //реализовать логику выдачи информации по локации по её ID
            //if (id == null || )
            return BadRequest();
        }
    }
}