using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace ETradeCoreBilgeAdam.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly CategoryServiceBase _categoryService;

        public CategoriesViewComponent(CategoryServiceBase categoryService)
        {
            _categoryService = categoryService;
        }

        public ViewViewComponentResult Invoke()
        {
            var categories = _categoryService.GetListAsync();
            var result = categories.Result;
            return View(result);
        }
    }
}
