using Simbir.Data.Interfaces;
using Simbir.Model;
using System.Data.Entity.Spatial;

namespace Simbir.Repository.Interfaces.RentInterfaces
{
    public interface IRentRepository : IBaseRepository<Rent>
    {
        Task<List<Rent>> SelectHistoryAccount(int idAccount);
        Task<int> GetIdAccount(string login);
        Task<List<Rent>> GetListTransportHistory(int idTransport, int idAccount);
        Task<List<Rent>> SearchTransport(DbGeography point, double radius);

    }
}
