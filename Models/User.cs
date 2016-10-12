using System;
using System.ComponentModel.DataAnnotations;

namespace Users.Models
{
    public abstract class BaseEntity {}

    public class User : BaseEntity
    {
        public int id;
        [Required]
        [MinLength(2)]
        public string first_name { get; set; }
        [Required]
        [MinLength(2)]
        public string last_name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [MinLength(8)]
        public string password { get; set; }               
        public DateTime created_at;
        public DateTime updated_at;
    }
}