using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.Data;
using NewsApp.Models;

namespace NewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly AppDbContext _context; // ← AppDbContext

        public ArticlesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            var articles = await _context.articles
                .Include(a => a.category)
                .Include(a => a.author)
                .ThenInclude(a => a.role)
                .OrderByDescending(a => a.published)
                .ToListAsync();

            return articles;
        }

        // GET: api/articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.articles
                .Include(a => a.category)
                .Include(a => a.author)
                .ThenInclude(a => a.role)
                .FirstOrDefaultAsync(a => a.id == id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // POST: api/articles
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            
            if (article.published == DateTime.MinValue)
                article.published = DateTime.UtcNow;// Устанавливаем текущее время

            
            var categoryExists = await _context.categories
                .AnyAsync(c => c.id == article.category_id);
            if (!categoryExists)
                return BadRequest("Category not found");

            
            var authorExists = await _context.users
                .AnyAsync(u => u.id == article.author_id);
            if (!authorExists)
                return BadRequest("Author not found");

            _context.articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticle), new { id = article.id }, article);
        }

        // PUT: api/articles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.id)
                return BadRequest();

            // Проверяем существование статьи
            var articleExists = await _context.articles.AnyAsync(a => a.id == id);
            if (!articleExists)
                return NotFound();

            // Проверяем существование категории
            var categoryExists = await _context.categories
                .AnyAsync(c => c.id == article.category_id);
            if (!categoryExists)
                return BadRequest("Category not found");

            // Проверяем существование автора
            var authorExists = await _context.users
                .AnyAsync(u => u.id == article.author_id);
            if (!authorExists)
                return BadRequest("Author not found");

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ArticleExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.articles.FindAsync(id);
            if (article == null)
                return NotFound();

            _context.articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ArticleExists(int id)
        {
            return await _context.articles.AnyAsync(e => e.id == id);
        }
    }
}