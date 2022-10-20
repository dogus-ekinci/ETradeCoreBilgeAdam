using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Services;

namespace ETradeCoreBilgeAdam.Controllers
{
    public class ShopsController : Controller
    {
        // Add service injections here
        private readonly ShopServiceBase _shopService;

        public ShopsController(ShopServiceBase shopService)
        {
            _shopService = shopService;
        }

        // GET: Shops
        public IActionResult Index()
        {
            List<Shop> shopList = _shopService.GetList(); // TODO: Add get list service logic here
            return View(shopList);
        }

        // GET: Shops/Details/5
        public IActionResult Details(int id)
        {
            Shop shop = _shopService.GetItem(id); // TODO: Add get item service logic here
            if (shop == null)
            {
                return NotFound();
            }
            return View(shop);
        }

        // GET: Shops/Create
        public IActionResult Create()
        {
            _shopService.GetList();

            return View();
        }

        // POST: Shops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Shop shop)
        {
            if (ModelState.IsValid)
            {
                var result = _shopService.Add(shop);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(shop);
        }

        // GET: Shops/Edit/5
        public IActionResult Edit(int id)
        {
            Shop shop = _shopService.GetItem(id); // TODO: Add get item service logic here
            if (shop == null)
            {
                return NotFound();
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(shop);
        }

        // POST: Shops/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Shop shop)
        {
            if (ModelState.IsValid)
            {
                var result = _shopService.Update(shop);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(shop);
        }

        // GET: Shops/Delete/5
        public IActionResult Delete(int id)
        {
            Shop shop = _shopService.GetItem(id); // TODO: Add get item service logic here
            if (shop == null)
            {
                return NotFound();
            }
            return View(shop);
        }

        // POST: Shops/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _shopService.Delete(c => c.Id == id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
