using AppCore.DataAccess.Services;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services.Bases
{
    public abstract class CountryServiceBase : ServiceBase<Country>
    {
        protected CountryServiceBase(Db dbContext) : base(dbContext)
        {
        }
    }
}
