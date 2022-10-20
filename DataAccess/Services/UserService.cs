using AppCore.DataAccess.Results;
using AppCore.DataAccess.Results.Bases;
using AppCore.DataAccess.Services;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public abstract class UserServiceBase : ServiceBase<User>
    {
        public UserServiceBase(Db dbContext) : base(dbContext)
        {
        }
    }

    public class UserService : UserServiceBase
    {
        public UserService(Db dbContext) : base(dbContext)
        {
        }

        public override IQueryable<User> Query()
        {
            return base.Query().Include(u => u.Role).Include(u => u.UserDetails);
        }

        public override Result Add(User entithy, bool save = true)
        {
            if (Query().Any(u => u.UserName == entithy.UserName))
                return new ErrorResult("User with same name exists!");
            if (Query().Any(u => u.UserDetails.Email == entithy.UserDetails.Email))
                return new ErrorResult("User with same e-mail exists!");
            return base.Add(entithy, save);
        }
        // herhangi bir entitie verisi çekiyorsan diğer entitideki verilerinde gelmesini istiyorsan include etmen lazım
    }
}
