using Simbir.Model;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Interfaces.RentInterface
{
    public interface IRentService
    {
        Task<IBaseResponse<List<Rent>>> GetListRent(string login, string jwtToken);
        Task<IBaseResponse<List<Rent>>> GetListTransportHistory(int idTransport, string login, string jwtToken);
        Task<IBaseResponse<HttpStatusCode>> AddNewRent (int idTransport, string price_type, string login, string jwtToken);
        Task<IBaseResponse<HttpStatusCode>> AddEndRent(string rentId, int lan, int lon, string login, string jwtToken);
        Task<IBaseResponse<Rent>> GetRent(string id, string login, string jwtToken);
        Task<IBaseResponse<List<Rent>>> SearchTransport(double lan, double lon, double radius, string transport_type);
    }
}
