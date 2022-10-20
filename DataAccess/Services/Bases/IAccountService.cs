using AppCore.DataAccess.Results.Bases;
using DataAccess.Entities;
using DataAccess.Models;

namespace DataAccess.Services.Bases
{
    public interface IAccountService
    {
        User Login(AccountLoginModel model);

        Result Register(AccountRegisterModel model);
    }
}
