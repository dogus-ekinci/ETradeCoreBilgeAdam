using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.Entities;
using DataAccess.Services.Bases;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using AppCore.Utils;

namespace ETradeCoreBilgeAdam.Controllers
{
    [Authorize] // hangi roller tanımlıysa onlar kullanabilecek yani Admin ve User
    public class ProductsController : Controller
    {
        // Add service injections here
        private readonly ProductServiceBase _productService;

        private readonly CategoryServiceBase _categoryService;

        private readonly ShopServiceBase _shopService;



        public ProductsController(ProductServiceBase productService, CategoryServiceBase categoryService, ShopServiceBase shopService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _shopService = shopService;
        }

        // GET: Products
        [AllowAnonymous]    // Sisteme girmiş olsada olmasada herkes görebilecek
        public IActionResult Index()
        {
            List<Product> productList = _productService.GetList(); // TODO: Add get list service logic here
            ViewData["Count"] = productList.Count == 0 ? "No records found." : productList.Count == 1 ? "1 record found." : productList.Count + " records found."; // alttakiyle aynı şey count yerine istediğin ismi verebilirsin
            ViewBag.Count = productList.Count == 0 ? "No records found." : productList.Count == 1 ? "1 record found." : productList.Count + " records found.";
            return View(productList);
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Admin")]    // Genel tanımladığımız authorize'yi ezip burdaki details için bunu tanımlar ve sadece admin görebilir index action'unu
        public IActionResult Details(int id)
        {
            Product product = _productService.GetItem(id); // TODO: Add get item service logic here
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [HttpGet]   // Get işlemi yazmasakta olur zaten default Get 
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            var categories = _categoryService.GetList();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");    // Create oluştururken dropdown list gerek o yüzden selectList yapıldı
            ViewBag.ShopIds = new MultiSelectList(_shopService.GetList(), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]  // post işlemi için bunu koy
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Product product, IFormFile productImage)
        {
            if (ModelState.IsValid)
            {
                var result = _productService.Add(product);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                    //return RedirectToAction("Index");
                }
                ModelState.TryAddModelError("", result.Message);    // validation summary
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewData["CategoryId"] = new SelectList(_categoryService.GetList(), "Id", "Name", product.CategoryId);    // Create oluştururken dropdown list gerek o yüzden selectList yapıldı
            ViewBag.ShopIds = new MultiSelectList(_shopService.GetList(), "Id", "Name", product.ShopIds);
            return View(product);
        }

        private bool? UpdateImage(Product entity, IFormFile uploadedFile)
        {
            #region Validation
            bool? result = null;
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                result = FileUtil.CheckFileExtension(uploadedFile.FileName, ".jpg, .jpeg, .pnp").IsSuccessful;
                if (result == true)
                {
                    result = FileUtil.CheckFileLength(uploadedFile.Length, 1).IsSuccessful;
                }
            }
            #endregion

            #region Dosyanın Kaydedilmesi
            if (result == true)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    uploadedFile.CopyTo(memoryStream);
                    entity.Image = memoryStream.ToArray();
                    entity.ImageExtension = Path.GetExtension(uploadedFile.FileName);
                }
            }
            #endregion

            return result;
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Product product = _productService.GetItem(id); // TODO: Add get item service logic here
            if (product == null)
            {
                return NotFound();
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewData["CategoryId"] = new SelectList(_categoryService.GetList(), "Id", "Name", product.CategoryId);
            ViewBag.ShopIds = new MultiSelectList(_shopService.GetList(), "Id", "Name", product.ShopIds);
            return View(product);
        }

        // POST: Products/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var result = _productService.Update(product);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewData["CategoryId"] = new SelectList(_categoryService.GetList(), "Id", "Name", product.CategoryId);
            ViewBag.ShopIds = new MultiSelectList(_shopService.GetList(), "Id", "Name", product.ShopIds);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var result = _productService.Delete(p => p.Id == id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }


    }
}
