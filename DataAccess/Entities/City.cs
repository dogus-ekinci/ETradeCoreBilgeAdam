using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class City : Record
    {
        [Required]
        [DisplayName("City Name")]
        [StringLength(200)]
        public string? Name { get; set; }

        [Required]
        [DisplayName("Country")]
        public int? CountryId { get; set; }

        public Country? Country { get; set; }

        public List<UserDetails>? UserDetails { get; set; }
    }
}
