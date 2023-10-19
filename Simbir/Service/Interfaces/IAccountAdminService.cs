using Simbir.Model;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Interfaces
{
    public interface IAccountAdminService
    {
        Task<IBaseResponse<HttpStatusCode>> SignUp(AccountAdminInput model);
        Task<IBaseResponse<Account>> GetAccount(string id);
        Task<IBaseResponse<HttpStatusCode>> DeleteAccount(int id);
    }
}
