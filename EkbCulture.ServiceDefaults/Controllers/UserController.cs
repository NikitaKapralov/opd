using EkbCulture.ServiceDefaults.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace EkbCulture.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsers()
        {
            // Логика получения пользователей
            return Ok(new List<User>());
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            // Логика создания пользователя
            return Ok(user);
        }
    }
}