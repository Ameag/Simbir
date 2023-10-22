using Simbir.Model;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Interfaces.AccountInterface
{
    public interface IAccountService
    {
        Task<IBaseResponse<Account>> GetAccount(string login, string jwtToken);
        Task<IBaseResponse<HttpStatusCode>> SignUp(AccountInput model);
        Task<IBaseResponse<string>> SignIn(AccountInput accountInput);
        Task<IBaseResponse<HttpStatusCode>> UpdateAccount(string login, AccountInput model, string jwtToken);
        Task<IBaseResponse<HttpStatusCode>> SignOut(string token);
    }
}
