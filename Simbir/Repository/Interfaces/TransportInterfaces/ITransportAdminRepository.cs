using Simbir.Data.Interfaces;
using Simbir.Model;

namespace Simbir.Repository.Interfaces.TransportInterfaces
{
    public interface ITransportAdminRepository : IBaseRepository<Transport>
    {
        Task<bool> СheckAdmin(string login);
        Task<bool> GetIdAccount(int id);
        Task<List<Transport>> SelectOfType(int start, int count, string type);
    }
}
