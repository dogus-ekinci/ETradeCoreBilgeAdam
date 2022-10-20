using AppCore.DataAccess.Results;
using AppCore.DataAccess.Results.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

public class ProductService : ProductServiceBase
{
    public ProductService(Db dbContext) : base(dbContext)
    {

    }

    public override IQueryable<Product> Query()     // product'ları çektiğimizde category gelmediği için ovverride edip Include ettik
    {
        // ThenInclude(ps => ps.Shop) 
        // gerek yok ancak ihtiyaç durumunda ProductShops üzerinden ilişkili shop verilerine ThenInclude ile ulaşılabilir
        return base.Query().Include(p => p.Category).Include(p => p.ProductShops).OrderBy(p => p.UnitPrice).ThenByDescending(p => p.Name).Select(p => new Product()   // Sıralama yaptım azalan OrderBy içinde yapabiliriz
        {   // Select ile parçalı ve orjinal yapıyı tanımladık ve özelleştirdik
            Id = p.Id,
            Name = p.Name,
            CategoryId = p.CategoryId,
            ExpirationDate = p.ExpirationDate,
            StockAmount = p.StockAmount,
            UnitPrice = p.UnitPrice,
            Description = p.Description,
            CategoryNameDisplay = p.Category.Name,
            ExpirationDateDisplay = p.ExpirationDate.HasValue ? p.ExpirationDate.Value.ToString("MM/dd/yyyy") : "",
            UnitPriceDisplay = p.UnitPrice != null ? p.UnitPrice.Value.ToString("C2") : "",    // C2 : ondalıktan sonra 2 hane gelecek anlamında
            ProductShops = p.ProductShops,
            ShopNamesDisplay = string.Join("<br />", p.ProductShops.Select(ps => ps.Shop.Name)),
            ShopIds = p.ProductShops.Select(ps => ps.ShopId ?? 0).ToList(),
            
        });
    }


    public override Result Add(Product entithy, bool save = true)
    {
        //var product = Query().SingleOrDefault(p => p.Name.ToLower() == entithy.Name.ToLower().Trim());
        //if (product != null)
        //    return new ErrorResult("The name you entered exists!");
        if (Query().Any(p => p.Name.ToLower() == entithy.Name.ToLower().Trim()))   // All
            return new ErrorResult("The name you entered exists!");

        entithy.ProductShops = entithy.ShopIds?.Select(sId => new ProductShop()
        {
            ShopId = sId
        }).ToList();

        entithy.Name = entithy.Name.Trim();
        entithy.Description = entithy.Description?.Trim();  // desc null olabileceği için ? bırakıp doluysada trimledik
        var result = base.Add(entithy, save);
        if (result.IsSuccessful)
            result.Message = "Product added successful.";   // appcoreden default string dönmesi yerine kendimiz tanımladık onu dönücez. Eğer web'i türkçe geliştirirsek bu şekilde yapılmalı
        return result;
    }


    public override Result Update(Product entithy, bool save = true)
    {
        if (Query().Any(p => p.Name.ToLower() == entithy.Name.ToLower().Trim() && p.Id != entithy.Id))
            return new ErrorResult("The name you entered exists!");
        entithy.Name = entithy.Name.Trim();
        entithy.Description = entithy.Description?.Trim();

        // ilk önce buraya gelen kullanıcı verileri içeren entity üzerinden  veritabanındaki ürün id üzerinden çekilir
        // (Query() methodunda mutlaka ProductShops sorguya dahil edilmeli). daha sonra ilişkili ProductSHops kayıtları _dbContext'teki ProductShops DbSet'i üzerindne silinir

        var product = Query().SingleOrDefault(p => p.Id == entithy.Id);
        _dbContext.Set<ProductShop>().RemoveRange(product.ProductShops);

        entithy.ProductShops = entithy.ShopIds?.Select(sId => new ProductShop()
        {
            ShopId = sId
        }).ToList();

        return base.Update(entithy, save);
    }

    public override Result Delete(Expression<Func<Product, bool>> predicate, bool save = true)
    {
        var product = Query().SingleOrDefault(predicate);
        _dbContext.Set<ProductShop>().RemoveRange(product.ProductShops);
        return base.Delete(predicate, save);
    }
}