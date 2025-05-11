using EkbCulture.AppHost.Data;
using EkbCulture.AppHost.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EkbCulture.AppHost.Services;
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
            var users = await _db.Users.ToListAsync();
            return Ok(users);
        }


        //GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == id);
            if (user == null) return NotFound("Пользователь не найден");
            return Ok(user);
        }

        //POST api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Проверка уникальности
                if (await _db.Users.AnyAsync(u => u.Email == user.Email))
                    return Conflict("Email уже занят");

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка сервера");
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
            return Ok(id);
        }



    }
}