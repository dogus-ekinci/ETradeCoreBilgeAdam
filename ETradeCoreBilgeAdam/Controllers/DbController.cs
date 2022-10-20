using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETradeCoreBilgeAdam.Controllers
{
    public class DbController : Controller
    {
        private readonly Db _db;

        public DbController(Db db)
        {
            _db = db;
        }

        public IActionResult Seed()
        {
            _db.UserDetails.RemoveRange(_db.UserDetails.ToList());  // ilk silinme sırası önemli user - city - country
            _db.Cities.RemoveRange(_db.Cities.ToList());
            _db.Countries.RemoveRange(_db.Countries.ToList());

            _db.Users.RemoveRange(_db.Users.ToList());

            var roles = _db.Roles.ToList();
            _db.Roles.RemoveRange(roles);

            if (roles.Count > 0)
                _db.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Roles', RESEED, 0)");
            // Bu komut database'ye ulaşıp id'lerin 1'den başlamasını sağlar işe yarar komuttur SQL Kodu. Şuan role üzerinden id leri sabitledik

            var productShops = _db.ProductShops.ToList();
            _db.ProductShops.RemoveRange(productShops);

            var shops = _db.Shops.ToList();
            _db.Shops.RemoveRange(shops);

            var products = _db.Products.ToList();
            _db.Products.RemoveRange(products);

            var categories = _db.Categories.ToList();
            _db.Categories.RemoveRange(categories);

            _db.Countries.Add(new Country()
            {
                Name = "Turkiye",
                Cities = new List<City>()
                {
                    new City()
                    {
                        Name = "İstanbul"
                    },
                    new City()
                    {
                        Name = "Ankara"
                    }
                }
            });

            _db.Countries.Add(new Country()
            {
                Name = "Kyrgyzstan",
                Cities = new List<City>()
                {
                    new City()
                    {
                        Name = "Bishkek"
                    }
                }
            });

            _db.SaveChanges();


            _db.Shops.Add(new Shop()
            {
                IsVirtual = true,
                Name = "Hepsiburada"
            });
            _db.Shops.Add(new Shop()
            {
                IsVirtual = false,
                Name = "Vatan"
            });

            _db.SaveChanges();

            _db.Categories.Add(new Category()
            {
                Name = "Computer",
                Description = "Laptops, desktops and computer peripherals",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Laptop",
                        UnitPrice = 3000.5,
                        ExpirationDate = new DateTime(2032, 1, 27),
                        StockAmount = 10,
                        ProductShops = new List<ProductShop>()
                        {
                            new ProductShop()
                            {
                                ShopId = _db.Shops.SingleOrDefault(s => s.Name == "Hepsiburada").Id
                            },
                            new ProductShop()
                            {
                                ShopId = _db.Shops.SingleOrDefault(s => s.Name == "Vatan").Id
                            }
                        }
                    },
                    new Product()
                    {
                        Name = "Mouse",
                        UnitPrice = 20.5,
                        StockAmount = 50,
                        Description = "Computer peripheral",
                        ProductShops = new List<ProductShop>()
                        {
                            new ProductShop()
                            {
                                ShopId = _db.Shops.SingleOrDefault(s => s.Name == "Hepsiburada").Id
                            }
                        }
                    },
                    new Product()
                    {
                        Name = "Keyboard",
                        UnitPrice = 40,
                        StockAmount = 45,
                        Description = "Computer peripheral"

                    },
                    new Product()
                    {
                        Name = "Monitor",
                        UnitPrice = 2500,
                        ExpirationDate = DateTime.Parse("05/19/2027"),
                        StockAmount = 20,
                        Description = "Computer peripheral"
                    }
                }
            });

            _db.Categories.Add(new Category()
            {
                Name = "Home Theater System",
                Description = "A perfect sound system.",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Speaker",
                        UnitPrice = 2500,
                        StockAmount = 70
                    },
                    new Product()
                    {
                        Name = "Receiver",
                        UnitPrice = 5000,
                        StockAmount = 30,
                        Description = "Home theater system component"
                    },
                    new Product()
                    {
                        Name = "Equalizer",
                        UnitPrice = 1000,
                        StockAmount = 40
                    }
                }
            });

            _db.Roles.Add(new Role()
            {
                Name = "Admin",
                Users = new List<User>()
                {
                    new User()
                    {
                        IsActive = true,
                        Password = "cagil",
                        UserName = "Çağıl",
                        UserDetails = new UserDetails()
                        {
                            Adress = "Çankaya",
                            Email = "cagil@cagil.com",
                            //Sex = DataAccess.Enums.Sex.Male,
                            Sex = Sex.Male,  // veya böyle using ekleyerek ekleyebiliriz
                            CountryId = _db.Countries.SingleOrDefault(c => c.Name == "Turkiye").Id,
                            CityId = _db.Cities.SingleOrDefault(c => c.Name == "Ankara").Id
                        }
                    }
                }
            });
            _db.Roles.Add(new Role()
            {
                Name = "user",
                Users = new List<User>()
                {
                    new User()
                    {
                        IsActive = true,
                        Password = "asil",
                        UserName = "Aşil",
                         UserDetails = new UserDetails()
                        {
                            Adress = "Çankaya",
                            Email = "asil@asil.com",
                            Sex = Sex.Male,
                            CountryId = _db.Countries.SingleOrDefault(c => c.Name == "Kyrgyzstan").Id,
                            CityId = _db.Cities.SingleOrDefault(c => c.Name == "Bishkek").Id
                        }
                    }
                }
            });


            _db.SaveChanges();
            return Content("<label style=\"color: red;\"><b>Database seed successful.</b></label>", "text/html");
        }
    }
}
