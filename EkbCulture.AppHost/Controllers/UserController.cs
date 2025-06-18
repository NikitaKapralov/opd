using EkbCulture.AppHost.Data;
using EkbCulture.AppHost.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EkbCulture.AppHost.Services;
using EkbCulture.AppHost.Dtos;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EkbCulture.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {

        private readonly AppDbContext _db;

        // Конструктор (внедрение зависимости)
        public UserController(AppDbContext db)
        {
            _db = db;
        }

        //GET: api/users/
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _db.Users
                 .Select(user => new UserResponseDto
                 {
                     Id = user.Id,
                     Username = user.Username,
                     Email = user.Email,
                     Level = user.Level,
                     Icon = user.IconUrl,
                     VisitedLocations = user.VisitedLocations
                 })
                 .ToListAsync();
            return Ok(users);
        }


        //GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == id);
            if (user == null) return NotFound("Пользователь не найден");

            var response = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Level = user.Level,
                Icon = user.IconUrl,
                VisitedLocations = user.VisitedLocations
            };
            return Ok(response);
        }

        //GET: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Невалидные данные", errors = ModelState.Values.SelectMany(v => v.Errors) });

                // Поиск пользователя по логину
                var user = await _db.Users
                    .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

                if (user == null)
                    return Unauthorized(new { message = "Пользователь не найден" });

                // Проверка пароля
                bool isPasswordValid = PasswordHasherService.VerifyPassword(
                    loginDto.Password,
                    user.PasswordHash
                );

                if (!isPasswordValid)
                    return Unauthorized(new { message = "Неверный пароль" });

                // Успешная авторизация - возвращаем основные данные
                var response = new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Level = user.Level,
                    VisitedLocations = user.VisitedLocations,
                    Icon = user.IconUrl
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка авторизации: {ex.Message}");
                return StatusCode(500, new { message = "Ошибка сервера", error = ex.Message });
            }
        }

        //POST api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Невалидная модель");

                // Проверка уникальности
                if (await _db.Users.AnyAsync(u => u.Email == userDto.Email))
                    return Conflict("Email уже занят");

                var user = new User
                (
                    userDto.Username,
                    userDto.Email,
                    PasswordHasherService.HashPassword(userDto.Password)
                );

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                //возвращаем без хеша
                var response = new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Level = user.Level,
                    Icon = user.IconUrl,
                    VisitedLocations = user.VisitedLocations
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                //логирование
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, new
                {
                    message = "Ошибка сервера",
                    error = ex.Message
                });
            }
        }

        //DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent(); // Стандартный REST-ответ для DELETE
        }


        //PATCH: api/users/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> ChangeUserData(int id,
            [FromBody] Dictionary<string, object> updates) //где string - название поля, object - новое значение поля
        {
            // Находим локацию по ID
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            // Перебираем все поля для обновления
            foreach (var update in updates)
            {
                // Ищем свойство в классе User по имени
                var property = typeof(User).GetProperty(
                    update.Key,
                    BindingFlags.IgnoreCase //игнорируем регистр
                    | BindingFlags.Public //только публичные свойства
                    | BindingFlags.Instance //не статические свойства
                );

                // Если свойство найдено, обновляем его значение
                if (property != null)
                    property.SetValue(user, //устанавливаем нвоое значение
                        Convert.ChangeType(update.Value, property.PropertyType)); //меняем тип с obj на нужный

            }

            // Сохраняем изменения в БД
            await _db.SaveChangesAsync();

            var response = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Level = user.Level,
                Icon = user.Icon,
                VisitedLocations = user.VisitedLocations
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
        }

        //Добавление/обновление аватара
        [HttpPatch("{id}/avatar")]
        public async Task<IActionResult> UpdateAvatar(int id, IFormFile avatarFile)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (avatarFile == null || avatarFile.Length == 0)
                return BadRequest("Файл не предоставлен");

            // Создаем папку для аватарок, если её нет
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            // Генерируем уникальное имя файла
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(avatarFile.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            // Сохраняем файл
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(stream);
            }

            // Обновляем URL аватара
            user.Icon = $"/avatars/{fileName}";
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Аватар обновлён",
                avatarUrl = user.Icon
            });
        }

        //Добавление посещенной локации
        [HttpPatch("{id}/add-location/{locationId}")]
        public async Task<IActionResult> AddVisitedLocation(int id, int locationId)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound("Пользователь не найден");

            // Проверяем, есть ли уже этот ID
            if (user.VisitedLocations.Contains(locationId))
                return Conflict("Локация уже посещена");

            // Добавляем ID локации
            var newVisited = user.VisitedLocations.ToList();
            newVisited.Add(locationId);
            user.VisitedLocations = newVisited.ToArray();

            // Обновляем уровень
            user.UpdateLevel();

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Локация добавлена в посещенные",
                visitedLocations = user.VisitedLocations,
                newLevel = user.Level
            });
        }

        //Обновление юзернейма
        [HttpPatch("{id}/username")]
        public async Task<IActionResult> UpdateUsername(int id, [FromBody] string newUsername)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(newUsername))
                return BadRequest("Имя пользователя не может быть пустым");

            user.Username = newUsername;
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Имя пользователя обновлено",
                newUsername = user.Username
            });
        }

        //Обновление почты
        [HttpPatch("{id}/email")]
        public async Task<IActionResult> UpdateEmail(int id, [FromBody] string newEmail)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (!new EmailAddressAttribute().IsValid(newEmail))
                return BadRequest("Некорректный email");

            if (await _db.Users.AnyAsync(u => u.Email == newEmail))
                return Conflict("Email уже занят");

            user.Email = newEmail;
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Email обновлён",
                newEmail = user.Email
            });
        }
    }
}