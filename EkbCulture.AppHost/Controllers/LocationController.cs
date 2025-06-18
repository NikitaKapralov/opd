using EkbCulture.AppHost.Data;
using EkbCulture.AppHost.Models;
using Microsoft.AspNetCore.Http;
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

        // GET: api/locations/search?name=парк
        [HttpGet("search")]
        public async Task<IActionResult> SearchLocations([FromQuery] string name)
        {
            // Если запрос пустой, возвращаются ВСЕ локации
            if (string.IsNullOrWhiteSpace(name))
                return Ok(await _db.Locations.ToListAsync());

            // Поиск по частичному совпадению (без учета регистра)
            var locations = await _db.Locations
                .Where(l => l.Name.ToLower()
                .Contains(name.ToLower()))
                .ToListAsync();

            return Ok(locations);
        }

        // POST: api/location
        [HttpPost]
        public async Task<IActionResult> Post(Location location)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
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


        [HttpPatch("{id}/image/{imageNumber}")]
        public async Task<IActionResult> UpdateImage(int id, int imageNumber, IFormFile imageFile)
        {
            var location = await _db.Locations.FindAsync(id);
            if (location == null) return NotFound();

            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("Файл не предоставлен");

            // Создаем папку для изображений локаций
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "location-images");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            // Генерируем уникальное имя файла
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            // Сохраняем файл
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Обновляем нужное изображение
            var imageUrl = $"/location-images/{fileName}";
            switch (imageNumber)
            {
                case 1: location.FirstImageUrl = imageUrl; break;
                case 2: location.SecondImageUrl = imageUrl; break;
                case 3: location.ThirdImageUrl = imageUrl; break;
                default: return BadRequest("Номер изображения должен быть 1, 2 или 3");
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = $"Изображение #{imageNumber} обновлено",
                imageUrl
            });
        }

        //Добавление посещения от юзера с userId
        [HttpPatch("{id}/visit/{userId}")]
        public async Task<IActionResult> AddVisitor(int id, int userId)
        {
            var location = await _db.Locations.FindAsync(id);
            if (location == null) return NotFound("Локация не найдена");

            var user = await _db.Users.FindAsync(userId);
            if (user == null) return NotFound("Пользователь не найден");

            // Если массив не инициализирован, создаем пустой список
            if (location.VisitedBy == null)
                location.VisitedBy = Array.Empty<int>();

            // Проверяем, не посещал ли уже
            if (location.VisitedBy.Contains(userId))
                return Conflict("Пользователь уже посещал эту локацию");

            // Добавляем пользователя
            var newVisitedBy = location.VisitedBy.ToList();
            newVisitedBy.Add(userId);
            location.VisitedBy = newVisitedBy.ToArray();

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Пользователь добавлен в список посетителей",
                visitedBy = location.VisitedBy
            });
        }


        [HttpPatch("{id}/rate")]
        public async Task<IActionResult> AddRating(int id, [FromBody] int rating)
        {
            if (rating < 1 || rating > 5)
                return BadRequest("Рейтинг должен быть от 1 до 5");

            var location = await _db.Locations.FindAsync(id);
            if (location == null) return NotFound();

            // Если массив оценок не инициализирован, создаем
            if (location.Ratings == null)
                location.Ratings = new List<int>();

            // Добавляем оценку
            location.Ratings.Add(rating);

            // Обновляем общий рейтинг
            location.UpdateRating();

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Оценка добавлена",
                newRating = location.Rating,
                totalRatings = location.Ratings.Count
            });
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
            return NoContent(); //стандартный ответ при удалении
        }
    }
}