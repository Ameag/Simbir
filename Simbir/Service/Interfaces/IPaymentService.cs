using Simbir.Data.Interfaces;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Interfaces
{
    public interface IPaymentService
    {
        Task<IBaseResponse<HttpStatusCode>> Hesoyam(string login);
    }
}
