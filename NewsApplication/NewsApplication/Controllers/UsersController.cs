using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.Data;
using NewsApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.users
                .Include(u => u.role)
                .ToListAsync();
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.users
                .Include(u => u.role)
                .FirstOrDefaultAsync(u => u.id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/users/by-username/Nick
        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            var user = await _context.users
                .Include(u => u.role)
                .FirstOrDefaultAsync(u => u.username == username);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
