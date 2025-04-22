using EkbCulture.AppHost.Data;
using EkbCulture.AppHost.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EkbCulture.Controllers
{
    [Route("api/[controller]")]  // Маршрутизация
    [ApiController]              // Поведение API
    public class LocationController : ControllerBase
    {
        private readonly AppDbContext _db;  // Контекст БД

        // Конструктор (внедрение зависимости)
        public LocationController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/location
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var locations = await _db.Locations.ToListAsync();
            return Ok(locations);
        }

        // POST: api/location
        [HttpPost]
        public async Task<IActionResult> Post(Location location)
        {
            _db.Locations.Add(location);
            await _db.SaveChangesAsync();
            return Ok(location);
        }
    }
}