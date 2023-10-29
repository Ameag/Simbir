using Simbir.Data.Interfaces;
using Simbir.Model;

namespace Simbir.Repository.Interfaces.RentInterfaces
{
    public interface IRentAdminRepository : IBaseRepository<Rent>
    {
        Task<List<Rent>> GetUserHistory(int userId);
        Task<List<Rent>> GetListTransportHistory(int transportId);
        Task<bool> СheckAdmin(string login);
    }
}
