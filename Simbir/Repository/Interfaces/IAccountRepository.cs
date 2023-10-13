using Simbir.Model;

namespace Simbir.Data.Interfaces
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<Account> SignIn(Account entity);
    }
}
