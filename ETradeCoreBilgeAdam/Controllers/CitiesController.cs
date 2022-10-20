using DataAccess.Services.Bases;
using Microsoft.AspNetCore.Mvc;

namespace ETradeCoreBilgeAdam.Controllers
{
    public class CitiesController : Controller
    {
        private readonly CityServiceBase _cityService;

        public CitiesController(CityServiceBase cityService)
        {
            _cityService = cityService;
        }
        public IActionResult GetJson(int countryId) // /cities/GetJson?countryId=1
        {
            // var cities = _cityService.GetList().Where(c => c.CountryId == countryId).ToList();
            var cities = _cityService.GetList(c => c.CountryId == countryId);
            return Json(cities);
        }
    }
}
