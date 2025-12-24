using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.Data;
using NewsApp.Models;
using System;
using System.Threading.Tasks;

namespace NewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context; // ← AppDbContext

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(LoginRequest request)
        {
            // Ищем пользователя по username и паролю
            var user = await _context.users
                .Include(u => u.role)
                .FirstOrDefaultAsync(u =>
                    u.username == request.Username &&
                    u.password_hash == request.Password); // ← password_hash

            if (user == null)
            {
                return Unauthorized(new { message = "Неверные учетные данные" });
            }

            // Получаем имя роли
            var roleName = user.role?.name ?? "Reader";

            // Определяем права на основе роли
            var canView = true;
            var canEdit = roleName == "Author" || roleName == "Admin";
            var canDelete = roleName == "Admin";

            // Возвращаем данные пользователя и его права
            return new
            {
                user.id,
                user.username,
                user.email,
                role = roleName,
                canView,
                canEdit,
                canDelete
            };
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}