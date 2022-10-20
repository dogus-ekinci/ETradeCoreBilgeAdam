using AppCore.DataAccess.Results;
using AppCore.DataAccess.Results.Bases;
using AppCore.DataAccess.Services;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Services
{
    public abstract class CategoryServiceBase : ServiceBase<Category>   // burası base
    {
        protected CategoryServiceBase(Db db) : base(db) // program.cs de Db üzerinden gönderidiğimiz için Db alıyoruz
        {

        }
    }

    public class CategoryService : CategoryServiceBase  // Dosyaları ayırmak yerine 2 dosyayı aynı class'ta yazdık tek fark bu 
    {
        public CategoryService(Db db) : base(db)
        {

        }

        public override IQueryable<Category> Query()
        {
            return base.Query().Include(c => c.Products).OrderBy(p => p.Name).Select(c => new Category()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Products = c.Products,  // alt tarafta Delete işlemi kontrolü yaparken product null döndüğü için burada da  tanımladık null olmması için

                ProductCountDisplay = c.Products == null ? 0 : c.Products.Count
            });
        }

        public override Result Add(Category entithy, bool save = true)
        {
            if (Query().Any(c => c.Name.ToUpper() == entithy.Name.ToUpper().Trim()))
                return new ErrorResult("Category with same name exists!");
            entithy.Name = entithy.Name.Trim();
            entithy.Description = entithy.Description?.Trim();
            return base.Add(entithy, save);
        }

        public override Result Update(Category entithy, bool save = true)
        {
            if (Query().Any(c => c.Name.ToUpper() == entithy.Name.ToUpper().Trim() && c.Id != entithy.Id))
                return new ErrorResult("Category with same name exists!");
            entithy.Name = entithy.Name.Trim();
            entithy.Description = entithy.Description?.Trim();
            return base.Update(entithy, save);
        }

        public override Result Delete(Expression<Func<Category, bool>> predicate, bool save = true)
        {
            var category = Query().SingleOrDefault(predicate);
            if (category.Products != null && category.Products.Count > 0)
                return new ErrorResult("Category cannot be deleted, it has products!");
            return base.Delete(predicate, save);
        }

    }
}
