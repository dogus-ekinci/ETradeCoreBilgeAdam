using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class ProductShop    // Record'tan miras almayacak ara tablo olduğu için id olmaması lazım
    {
        [Key]   // özelliği Id verseydik primarykey olduğunu anlardı fakat biz Key verdik bu şekilde primarykey yaptık
        [Column(Order= 0)]  // Sıra belirttik, çok kurcalama
        public int? ProductId { get; set; }

        public Product? Product { get; set; }   // product ve shop tanımlayarak içeriğinede ulaşabilecez yani referans oluşturduk ve ilişkili tablolara ulaşabildik. Id olanlar ise ara tablo oluşturmak içindir
        

        [Key]
        [Column(Order = 1)]
        public int? ShopId { get; set; }

        public Shop? Shop { get; set; }
    }
}
