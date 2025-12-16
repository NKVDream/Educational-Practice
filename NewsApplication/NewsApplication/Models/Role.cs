// Models/Role.cs
//using NewsApplication.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; // Admin, Author, Reader

        public string? Description { get; set; }

        // Навигационное свойство
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
