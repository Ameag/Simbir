using Simbir.Data.Interfaces;
using Simbir.Model;
using System.Net;

namespace Simbir.Repository.Interfaces.AccountInterfaces
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<Account> SignIn(Account entity);

    }
}
