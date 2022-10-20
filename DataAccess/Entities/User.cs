using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class User : Record
    {
        [Required]
        [MinLength(2)]
        [MaxLength(15)]
        [DisplayName("User Name")]
        public string? UserName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string? Password { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [Required]
        public int? RoleId { get; set; }

        public Role? Role { get; set; }

        public UserDetails? UserDetails { get; set; }
    }
}
