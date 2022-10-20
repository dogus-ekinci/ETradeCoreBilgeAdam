using AppCore.DataAccess.Services;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace DataAccess.Services.Bases
{
    public abstract class ProductServiceBase : ServiceBase<Product>
    {
        protected ProductServiceBase(Db dbContext) : base(dbContext)   // program.cs dosyasındaki ıos ye db belirttiğimiz için buranın da db alması lazım
        {
        }
    }
}
