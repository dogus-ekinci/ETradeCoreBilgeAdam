using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public partial class Category : Record
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100)]     // ErrorMessage vermeye gerek yok alacağı değeri vermek yeterli olur
        public string? Name { get; set; }


        [DisplayName("Category Description")]
        [StringLength(500, ErrorMessage = "{0} must have maximum {1} characters!")]
        public string? Description { get; set; }


        public List<Product>? Products { get; set; }
    }

    public partial class Category
    {
        [NotMapped]
        [DisplayName("Product Count")]
        public int? ProductCountDisplay { get; set; }
    }
}
