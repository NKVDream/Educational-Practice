// DTOs/ArticleDto.cs
using System.ComponentModel.DataAnnotations;

namespace NewsApp.DTOs
{
    public class ArticleDto
    {
        public int? id { get; set; }

        [Required(ErrorMessage = "Заголовок обязателен")]
        [MaxLength(200, ErrorMessage = "Заголовок не должен превышать 200 символов")]
        public string title { get; set; }

        [Required(ErrorMessage = "Содержание обязательно")]
        public string content { get; set; }

        public string excerpt { get; set; }

        [MaxLength(500, ErrorMessage = "Ссылка на изображение не должна превышать 500 символов")]
        public string cover_image_url { get; set; }

        public DateTime? published { get; set; }

        [Required(ErrorMessage = "Категория обязательна")]
        [Range(1, int.MaxValue, ErrorMessage = "Выберите категорию")]
        public int category_id { get; set; }

        [Required(ErrorMessage = "Автор обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "Автор не указан")]
        public int author_id { get; set; }
    }
}