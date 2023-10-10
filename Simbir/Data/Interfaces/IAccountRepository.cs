using Simbir.Model;

namespace Simbir.Data.Interfaces
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Account GetAccount(int id);
    }
}
