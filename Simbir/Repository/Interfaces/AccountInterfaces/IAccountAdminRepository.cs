using Simbir.Data.Interfaces;
using Simbir.Model;

namespace Simbir.Repository.Interfaces.AccountInterfaces
{
    public interface IAccountAdminRepository : IBaseRepository<Account>
    {
        Task<bool> СheckAdmin(string login);
    }
}
