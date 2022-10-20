using DataAccess.Contexts;
using DataAccess.Services.Bases;

namespace DataAccess.Services
{
    public class CountryService : CountryServiceBase
    {
        public CountryService(Db dbContext) : base(dbContext)
        {
        }
    }
}
