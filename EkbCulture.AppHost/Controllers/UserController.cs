using EkbCulture.AppHost.Data;
using EkbCulture.AppHost.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EkbCulture.AppHost.Services;
using EkbCulture.AppHost.Dtos;
using System.Reflection;

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
                     Icon = user.Icon,
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
                Icon = user.Icon,
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
                    Icon = user.Icon
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
                    Icon = user.Icon,
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
    }
}