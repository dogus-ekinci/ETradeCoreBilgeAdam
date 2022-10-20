using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Country : Record
    {
        [Required]
        [DisplayName("Country Name")]
        [StringLength(150)]
        public string? Name { get; set; }

        public List<City>? Cities { get; set; }

        public List<UserDetails>? UserDetails { get; set; }
    }
}
