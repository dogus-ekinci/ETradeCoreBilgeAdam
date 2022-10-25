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

        public void DeleteImage(int id) // productService tekoymamıza rağmen çağıramadık base koyduk
        {
            var product = Query().SingleOrDefault(p => p.Id == id);
            product.Image = null;
            product.ImageExtension = null;
            base.Update(product);
            // Burda Save() çalışmadı veya Update() Update methodu override edildiği için orjinal methodu çağıramadık direk özelleştirilmiş override update'i çağırdı bu yüzden base diye belirtmemiz  gerekti
        }
    }
}
