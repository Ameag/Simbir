using Simbir.Data.Interfaces;
using Simbir.Model;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Interfaces.TransportInterface
{
    public interface ITransportService
    {
        Task<IBaseResponse<HttpStatusCode>> AddTransport(TransportInput model, string jwtToken, string login);
        Task<IBaseResponse<Transport>> GetTransport(string id);
        Task<IBaseResponse<HttpStatusCode>> UpdateTransport(TransportInput model, string id, string jwtToken, string login);
        Task<IBaseResponse<HttpStatusCode>> DeleteTransport(int id, string jwtToken, string login);
    }
}
