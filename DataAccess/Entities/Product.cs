using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public partial class Product : Record
    {
        [Required(ErrorMessage = "{0} is required!")]    // {0} aslında DisplayName'yi işaret eder yani Name'yi
        [DisplayName("Product Name")]
        [MinLength(3, ErrorMessage = "{0} must have minimum {1} characters!")]  // {1} minlength içindeki 3'ü işaret eder
        [MaxLength(200, ErrorMessage = "{0} must have maximum {1} characters!")]  // {1} 200'ü işaret eder
        public string? Name { get; set; }


        [DisplayName("Product Description")]
        public string? Description { get; set; }


        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, 1000000, ErrorMessage = "{0} must be between {1} and {2}!")]
        [DisplayName("Stock Amount")]
        public int? StockAmount { get; set; }


        [DisplayName("Unit Price")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} must be between {1} and {2}!")]
        [Required(ErrorMessage = "{0} is required!")]
        public double? UnitPrice { get; set; }


        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }


        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Category")]
        public int? CategoryId { get; set; }    // dropdown üzerinden id alabilmek için


        public Category? Category { get; set; }

        public List<ProductShop>? ProductShops { get; set; } // many to many ilişki için tanımladık

        [Column(TypeName = "image")]
        public byte[]? Image { get; set; }

        [StringLength(5)]
        public string? ImageExtension { get; set; }  // .jpg, .bmp, .pnp
    }

    public partial class Product    // parçaladık
    {
        [NotMapped]     // add-migration yapıldığında veritabanında oluşmaması için yazdık. Sadece özelliği özelleştirmek için yapıyoruz bu class'ı
        [DisplayName("Unit Price")]
        public string? UnitPriceDisplay { get; set; }


        [NotMapped]
        [DisplayName("Expiration Date")]
        public string? ExpirationDateDisplay { get; set; }


        [NotMapped]
        [DisplayName("Category")]
        public string? CategoryNameDisplay { get; set; }


        [NotMapped]
        [DisplayName("Shops")]
        //[Required]
        public List<int>? ShopIds { get; set; }    // listbox üzerinden id'leri alabilmek için

        [NotMapped]
        [DisplayName("Shops")]
        public string? ShopNamesDisplay { get; set; }

        [NotMapped]
        [DisplayName("Image")]
        public string? ImageTagSrcDisplay { get; set; }
    }
}
