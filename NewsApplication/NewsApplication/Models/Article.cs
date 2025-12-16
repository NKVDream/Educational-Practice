// Models/Article.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string? Excerpt { get; set; }

        [MaxLength(500)]
        public string? CoverImageUrl { get; set; }

        public DateTime Published { get; set; } = DateTime.Now;

        // Внешние ключи
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }

        // Навигационные свойства
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [ForeignKey("AuthorId")]
        public User? Author { get; set; }
    }
}
