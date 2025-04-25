using EkbCulture.AppHost.Data;
using EkbCulture.AppHost.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

        // GET: api/locations
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var locations = await _db.Locations.ToListAsync();
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _db.Locations.SingleOrDefaultAsync(x => x.Id == id);
            if (location == null)
                return NotFound($"Not Found {id} location");
            return Ok(location);
        }

        // POST: api/location
        [HttpPost]
        public async Task<IActionResult> Post(Location location)
        {
            try
            {
                _db.Locations.Add(location);
                await _db.SaveChangesAsync();
                return Ok(location);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PATCH: api/location/id
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, 
            [FromBody] Dictionary<string, object> updates) //где string - название поля, object - новое значение поля
        {
            // Находим локацию по ID
            var location = await _db.Locations.FindAsync(id);
            if (location == null)
                return NotFound();

            // Перебираем все поля для обновления
            foreach (var update in updates)
            {
                // Ищем свойство в классе Location по имени
                var property = typeof(Location).GetProperty(
                    update.Key,
                    BindingFlags.IgnoreCase //игнорируем регистр
                    | BindingFlags.Public //только публичные свойства
                    | BindingFlags.Instance //не статические свойства
                );

                // Если свойство найдено, обновляем его значение
                if (property != null)
                    property.SetValue(location, //устанавливаем нвоое значение
                        Convert.ChangeType(update.Value, property.PropertyType)); //меняем тип с obj на нужный
                
            }

            // Сохраняем изменения в БД
            await _db.SaveChangesAsync();
            return Ok(location);
        }

        //DELETE: api/location/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var location = await _db.Locations.FindAsync(id);
            if (location == null)
                return NotFound();
            _db.Locations.Remove(location);
            await _db.SaveChangesAsync();
            return Ok(id);
        }
    }
}