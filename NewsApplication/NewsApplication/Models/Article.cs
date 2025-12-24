// Models/Article.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models
{
    public class Article
    {
        public int id { get; set; }  // точно как в таблице

        [Required]
        [MaxLength(200)]
        public string title { get; set; }

        [Required]
        public string content { get; set; }

        public string excerpt { get; set; }

        [MaxLength(500)]
        public string cover_image_url { get; set; }

        public DateTime published { get; set; } = DateTime.UtcNow;

        public int category_id { get; set; }  // точно как внешний ключ

        public int author_id { get; set; }    // точно как внешний ключ

        // Навигационные свойства
        public Category category { get; set; }
        public User author { get; set; }
    }
}
