using DataAccess.Models;
using DataAccess.Services.Bases;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ETradeCoreBilgeAdam.Areas.Accounts.Controllers
{
    [Area("Accounts")]  // Area tanımlaman gerekli yoksa çalışmıyor
    public class HomeController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly CountryServiceBase _countryService;
        private readonly CityServiceBase _cityService;

        public HomeController(IAccountService accountService, CountryServiceBase countryService, CityServiceBase cityService)
        {
            _accountService = accountService;
            _countryService = countryService;
            _cityService = cityService;
        }

        public IActionResult Login()
        {
            return View("LoginNew");
        }


        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginModel model) // await kullandığımız methodu async yazıp dönüş tipini Task olarak yazmamız lazım
        {   // Login olurken hangi verilerin çerezler kısmında tutulacağını belirttik
            if (ModelState.IsValid)
            {
                var user = _accountService.Login(model);
                if (user != null)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.Role.Name),
                        new Claim(ClaimTypes.Actor, user.UserName)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal); // asenkron çalışıyor ama kullanıcıyı bekletmemiz lazım await belirledik
                    return RedirectToAction("Index", "Home", new { area = "" });  // 3.param olarak area null verdik yoksa homecontrollere yönlendiremiyoruz
                }
            }
            return View("LoginNew", model);
        }

        public async Task<IActionResult> Logout()       // Logout işlemi için
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home", new { area = " " });
        }

        public IActionResult AccessDenied()
        {
            return View("_Error", "Acces is denied");
        }

        public IActionResult Register()
        {
            // SelectList'in 1.parametresi : koleksiyon, 2.parametresi : koleksiyondaki model üzerinden (Country) dropdownlist'te değer olarak kullanılacak özellik, 3.parametre : koleksiyondaki model üzerinden (Country) dropdownlist'te yazı (kullanıcıya gösterilecek) olarak kullanılacak özellik
            ViewBag.Countries = new SelectList(_countryService.Query().OrderBy(c => c.Name).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(AccountRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Register(model);   // registeri tanımlamak için IAccountService'te tanımlamamız lazım      

                if (result.IsSuccessful)
                {
                    //return RedirectPermanent("https://microsoft.com");
                    return RedirectToRoute("welcome"); // register.cs de tanımladık nereye yönlendireceğini
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.Countries = new SelectList(_countryService.Query().OrderBy(c => c.Name).ToList(), "Id", "Name", model.CountryId);
            ViewBag.Cities = new SelectList(_cityService.GetList(c => c.CountryId == model.CountryId), "Id", "Name", model.CityId);
            return View(model);
        }
    }
}
