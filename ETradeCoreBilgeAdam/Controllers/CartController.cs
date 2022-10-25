using DataAccess.Models;
using DataAccess.Services.Bases;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ETradeCoreBilgeAdam.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductServiceBase _productService;

        public CartController(ProductServiceBase productService)
        {
            _productService = productService;
        }

        public IActionResult AddToCart(int productId)
        {
            List<CartItemModel> cart = new List<CartItemModel>();
            string json = HttpContext.Session.GetString("cart");
            if (json != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartItemModel>>(json);
            }

            var product = _productService.GetItem(productId);
            int userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            CartItemModel item = new CartItemModel()
            {
                ProductId = productId,
                ProductName = product.Name,
                UnitPrice = product.UnitPrice,
                UserId = userId
            };
            cart.Add(item);
            json = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", json);
            TempData["Message"] = $"{product.Name} added to cart.";
            return RedirectToAction("Index", "Products");
        }
    }
}
