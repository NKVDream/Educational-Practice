// Models/Role.cs
//using NewsApplication.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models
{
    public class Role
    {
        public int id { get; set; }

        [Required]
        [MaxLength(50)]
        public string name { get; set; }  // 'Admin', 'Author', 'Reader'

        public string description { get; set; }
    }
}
