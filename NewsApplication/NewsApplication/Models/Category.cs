// Models/Category.cs
//using NewsApplication.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models
{
    public class Category
    {
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        public string description { get; set; }
    }
}
