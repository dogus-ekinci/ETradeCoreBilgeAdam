using DataAccess.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class UserDetails
    {
        [Key]
        public int? UserId { get; set; }
        public User? User { get; set; }

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

        public Country? Country { get; set; }

        [Required]
        [DisplayName("City")]
        public int? CityId { get; set; }

        public City? City { get; set; }
    }
}
