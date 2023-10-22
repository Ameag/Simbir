using Simbir.Data.Interfaces;
using Simbir.Model;

namespace Simbir.Repository.Interfaces.TransportInterfaces
{
    public interface ITransportRepository : IBaseRepository<Transport>
    {
        Task<int> GetIdAccount(string login);
    }
}
