using Microsoft.AspNetCore.Mvc;
using Simbir.Model;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Interfaces.RentInterface
{
    public interface IRentAdminService
    {
        Task<IBaseResponse<Rent>> GetRent(string id, string jwtToken, string login);
        Task<IBaseResponse<List<Rent>>> GetUserHistory(int userId, string jwtToken, string login);
        Task<IBaseResponse<List<Rent>>> GetTransportHistory(int transportId, string jwtToken, string login);
        Task<IBaseResponse<HttpStatusCode>> AddNewRent([FromBody] InputAdminRent model, string jwtToken,  string login);
        Task<IBaseResponse<HttpStatusCode>> AddEndRent(int rentId, string jwtToken, double lat, double lon, string login);
        Task<IBaseResponse<HttpStatusCode>> UpdateRent(int id, [FromBody] InputAdminRent model, string jwtToken, string login);
        Task<IBaseResponse<HttpStatusCode>> DeleteRent (int id, string jwtToken, string login);

    }
}
