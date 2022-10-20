using AppCore.DataAccess.Services;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services.Bases
{
    public abstract class CityServiceBase : ServiceBase<City>
    {
        protected CityServiceBase(Db dbContext) : base(dbContext)
        {
        }
    }
}
