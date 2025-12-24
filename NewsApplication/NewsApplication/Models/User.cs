using System.ComponentModel.DataAnnotations;

namespace NewsApp.Models
{
    public class User
    {
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string username { get; set; }

        [Required]
        [MaxLength(100)]
        public string email { get; set; }

        [Required]
        [MaxLength(255)]
        public string password_hash { get; set; } // ← ДОБАВЛЕНО

        public int user_role { get; set; }

        public Role role { get; set; }
    }
}