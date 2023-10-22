using Simbir.Model;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Interfaces.AccountInterface
{
    public interface IAccountAdminService
    {
        Task<IBaseResponse<HttpStatusCode>> SignUp(AccountAdminInput model, string login, string jwtToken);
        Task<IBaseResponse<Account>> GetAccount(string id, string login, string jwtToken);
        Task<IBaseResponse<HttpStatusCode>> DeleteAccount(int id, string login, string jwtToken);
        Task<IBaseResponse<HttpStatusCode>> UpdateAccount(string id, string login, string jwtToken, AccountAdminInput model);
        Task<IBaseResponse<List<Account>>> GetListAccount(string login, string jwtToken, int start, int count);
    }
}
