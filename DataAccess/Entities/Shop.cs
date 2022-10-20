using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    // Table("UrunMagaza")]     DataBase'de tablo ismi verebiliriz ama program kısmında burdaki isim kullanılır
    public partial class Shop : Record
    {
        [Required(ErrorMessage ="{0} is required!")]
        [StringLength(150, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Shop Name")]
        public string? Name { get; set; }

        [DisplayName("Virtual")]
        public bool IsVirtual { get; set; } // bool'larda ? koyamazsın

        public List<ProductShop>? ProductShops { get; set; } // many to many ilişki için tanımladık
    }

    public partial class Shop
    {
        [NotMapped]
        [DisplayName("Virtual")]
        public string? IsVirtualDisplay { get; set; }
    }
}
