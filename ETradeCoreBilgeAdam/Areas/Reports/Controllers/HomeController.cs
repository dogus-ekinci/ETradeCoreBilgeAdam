using DataAccess.Services;
using ETradeCoreBilgeAdam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETradeCoreBilgeAdam.Areas.Reports.Controllers
{
    [Area("Reports")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IReportService _reportService;
        private readonly CategoryServiceBase _categoryService;
        private readonly ShopServiceBase _shopService;

        public HomeController(IReportService reportService, CategoryServiceBase categoryService, ShopServiceBase shopService)
        {
            _reportService = reportService;
            _categoryService = categoryService;
            _shopService = shopService;
        }

        public IActionResult Index(HomeIndexViewModel viewModel)
        {
            //var model = _reportService.GetListInnerJoin();
            var model = _reportService.GetListLeftOuterJoin(viewModel.Filter);
            viewModel.Report = model;
            viewModel.Categories = new SelectList(_categoryService.GetList(), "Id", "Name");
            viewModel.Shops = new MultiSelectList(_shopService.GetList(), "Id", "Name");
            return View(viewModel);
        }
    }
}
