#nullable disable

using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETradeCoreBilgeAdam.Models
{
    public class HomeIndexViewModel
    {
        public List<ReportModel> Report { get; set; }
        public SelectList Categories { get; set; }  //List<Category> yerine SelectList kullandık normalde view'de zaten selectlist kullacağımıza burda kullandık
        public ReportFilterModel Filter { get; set; }
        public MultiSelectList Shops { get; set; }
    }
}
