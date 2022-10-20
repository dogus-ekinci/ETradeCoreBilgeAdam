using AppCore.DataAccess.Results;
using AppCore.DataAccess.Results.Bases;
using AppCore.DataAccess.Services;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Services
{
    public abstract class ShopServiceBase : ServiceBase<Shop>   // burası base
    {
        protected ShopServiceBase(Db db) : base(db) // program.cs de Db üzerinden gönderidiğimiz için Db alıyoruz
        {

        }
    }

    public class ShopService : ShopServiceBase  // Dosyaları ayırmak yerine 2 dosyayı aynı class'ta yazdık tek fark bu 
    {
        public ShopService(Db db) : base(db)
        {

        }


        public override IQueryable<Shop> Query()
        {
            return base.Query().OrderBy(s => s.Name).Select(s => new Shop()
            {
                Id = s.Id,
                IsVirtual = s.IsVirtual,
                Name = s.Name,

                IsVirtualDisplay = s.IsVirtual ? "Yes" : "No"
            });
        }

    }

}
