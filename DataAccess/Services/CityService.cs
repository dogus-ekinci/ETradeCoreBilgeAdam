using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class CityService : CityServiceBase
    {
        public CityService(Db dbContext) : base(dbContext)
        {
        }

        public override IQueryable<City> Query()
        {
            return base.Query().Include(c => c.Country).OrderBy(c => c.Name);
        }
    }
}
