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
            List<CartItemModel> cart = GetSession();
            string json;

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

        //[NonAction] // Bu methodu çağıramamak için NonAction belirttik ya da aşağıdaki gibi methodu private tanımlarız
        //public IActionResult Get();
        //private IActionResult Get();

        private List<CartItemModel> GetSession(string key = "cart")
        {
            List<CartItemModel> cart = new List<CartItemModel>();
            string json = HttpContext.Session.GetString("cart");
            if (json != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartItemModel>>(json);
            }
            return cart;
        }

        public IActionResult GetCart()  // add View ile bunları göstermemiz lazım  dolu olarak product entiti'sine List alarak
        {
            List<CartItemModel> cart = GetSession().Select(c => new CartItemModel() // Select IEnumrable döndüğü için bizim list almamız için tolist yapmamız lazım
            {
                ProductId = c.ProductId,
                ProductName = c.ProductName,
                UnitPrice = c.UnitPrice,
                UserId = c.UserId,
                UnitPriceDisplay = (c.UnitPrice ?? 0).ToString("C2")  // tostring verince double veri ? bıraktığı için bunu null ise 0 vereceksin ?? ile
            }).ToList();
            var groupBy = from c in cart
                          group c by new { c.ProductId, c.UserId, c.ProductName }
                          into cGoupBy
                          select new CartItemGroupByModel()
                          {
                              ProductId = cGoupBy.Key.ProductId ?? 0, // cartitemmodeldeki productid null değil ama diğeri ? tanımlı bu yüzden null'sa 0 olarak tanımladık ?? 0
                              UserId = cGoupBy.Key.UserId ?? 0,
                              ProductName = cGoupBy.Key.ProductName,
                              TotalPrice = cGoupBy.Sum(cgb => cgb.UnitPrice ?? 0),
                              ProductCount = cGoupBy.Count()
                          };
            return View(groupBy);
        }

        public IActionResult ClearCart()
        {
            //HttpContext.Session.Clear();
            //HttpContext.Session.Remove("cart");

            var cart = GetSession();
            int userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value); // homecontroller'deki claims içinde tanımladığımız side göre silme yapıcaz
            var userCart = cart.Where(C => C.UserId == userId).ToList();    // foreach ile readonly verileri döndüğümüz için silemeyiz bunun için tolist ile list oluşturup silmemiz lazım

            // fakat foreach yerine except ile daha kolay olur
            //foreach (var userCartItem in userCart)
            //{
            //    cart.Remove(userCartItem);
            //}

            /*userCart.ForEach(c => cart.Remove(c));*/  // veya bu şekilde  foreach tarzında da silebiliriz

            cart = cart.Except(userCart).ToList();  // sildik

            string json = JsonConvert.SerializeObject(cart);    // işlem bittikten sonra session'a atıyoruz
            HttpContext.Session.SetString("cart", json);
            return RedirectToAction(nameof(GetCart));
        }

        public IActionResult DeleteFromCart(int productId)
        {
            var cart = GetSession();
            int userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            var cartItem = cart.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);  // birden fazla aynı ürün varsa first kullanmamız lazım 

            cart.Remove(cartItem);  // sildik

            string json = JsonConvert.SerializeObject(cart);    // session'a gönderdikS
            HttpContext.Session.SetString("cart", json);
            return RedirectToAction(nameof(GetCart));
        }
    }
}
