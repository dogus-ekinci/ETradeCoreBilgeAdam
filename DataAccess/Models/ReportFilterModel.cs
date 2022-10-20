using System.ComponentModel;

namespace DataAccess.Models
{
    public class ReportFilterModel
    {
        [DisplayName("Category")]
        public int? CategoryId { get; set; }
        [DisplayName("Product Name")]
        public string? ProductName { get; set; }
        public List<int>? ShopIds { get; set; }

        [DisplayName("Expiration")]
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
