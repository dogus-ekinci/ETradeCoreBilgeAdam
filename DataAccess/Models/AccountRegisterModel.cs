using DataAccess.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class AccountRegisterModel
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

        [Required]
        public Sex? Sex { get; set; }

        [Required]
        [StringLength(250)]
        [EmailAddress]
        [DisplayName("E-Mail")]
        public string? Email { get; set; }

        [Required]
        [StringLength(1000)]
        public string? Adress { get; set; }

        [Required]
        [DisplayName("Country")]
        public int? CountryId { get; set; }

        [Required]
        [DisplayName("City")]
        public int? CityId { get; set; }
    }
}
