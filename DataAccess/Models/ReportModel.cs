using System.ComponentModel;

namespace DataAccess.Models
{
    public class ReportModel
    {
        [DisplayName("Product Name")]
        public string? ProductName { get; set; }

        [DisplayName("product Description")]
        public string? ProductDescription { get; set; }

        [DisplayName("Product Unit Price")]
        public double? ProductUnitPrice { get; set; }

        [DisplayName("Stock Amount")]
        public int? StockAmount { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Category Name")]
        public string? CategoryName { get; set; }

        [DisplayName("Shop Name")]
        public string? ShopName { get; set; }

        #region filter
        public int? CategoryId { get; set; }
        public int? ShopId { get; set; }
        #endregion

        [DisplayName("Expiration Date")]
        public string? ExpirationDateDisplay { get; set; }
    }
}