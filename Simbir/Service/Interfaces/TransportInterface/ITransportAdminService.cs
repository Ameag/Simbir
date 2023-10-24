using Simbir.Model;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Interfaces.TransportInterface
{
    public interface ITransportAdminService
    {
        Task<IBaseResponse<HttpStatusCode>> AddTransport(TransportInputAdmin model, string jwtToken, string login);
        Task<IBaseResponse<Transport>> GetTransport(string id, string jwtToken, string login);
        Task<IBaseResponse<HttpStatusCode>> DeleteTransport(int id, string jwtToken, string login);
        Task<IBaseResponse<List<Transport>>> GetListTransport(string login, string jwtToken, int start, int count, string type);
        Task<IBaseResponse<HttpStatusCode>> UpdateTransport(string id, TransportInputAdmin model, string jwtToken, string login);
    }
}
