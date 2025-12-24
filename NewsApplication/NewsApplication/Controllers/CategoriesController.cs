using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.Data;
using NewsApp.Models;

namespace NewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context; // ← AppDbContext

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.categories.ToListAsync();
        }
    }
}