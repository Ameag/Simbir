using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simbir.Model;
using Simbir.Service.Implementations.RentService;
using Simbir.Service.Interfaces.RentInterface;
using System.Net;

namespace Simbir.Controllers.RentController
{
    public class RentAdminController : Controller
    {
        private readonly IRentAdminService _rentAdminService;
        public RentAdminController(IRentAdminService rentAdminService)
        {
            _rentAdminService = rentAdminService;
        }

        [Authorize]
        [HttpGet("/api/Admin/Rent/{rentId}")]
        public async Task<ActionResult<Rent>> GetRent(string rentId)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;

            var response = await _rentAdminService.GetRent(rentId, jwtToken, login);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Data;
            }
        }

        [Authorize]
        [HttpGet("/api/Admin/UserHistory/{userId}")]
        public async Task<ActionResult<List<Rent>>> GetUserHistory(int userId)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;

            var response = await _rentAdminService.GetUserHistory(userId, jwtToken, login);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Data;
            }
        }

        [Authorize]
        [HttpGet("/api/Admin/TransportHistory/{transportId}")]
        public async Task<ActionResult<List<Rent>>> GetTransportHistory(int transportId)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;

            var response = await _rentAdminService.GetTransportHistory(transportId, jwtToken, login);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Data;
            }
        }

        [Authorize]
        [HttpPost("/api/Admin/Rent")]
        public async Task<ActionResult<HttpStatusCode>> AddNewRent([FromBody] InputAdminRent model)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;

            var response = await _rentAdminService.AddNewRent(model, jwtToken, login);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Status;
            }
        }
        [Authorize]
        [HttpPost("/api/Admin/Rent/End/{rentId}")]
        public async Task<ActionResult<HttpStatusCode>> AddEndRent(int rentId, double lat, double lon)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;

            var response = await _rentAdminService.AddEndRent(rentId, jwtToken, lat, lon, login);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Status;
            }
        }

        [Authorize]
        [HttpPut("/api/Admin/Rent/{id}")]
        public async Task<ActionResult<HttpStatusCode>> UpdateRent(int id, [FromBody] InputAdminRent model)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;

            var response = await _rentAdminService.UpdateRent(id, model, jwtToken, login);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Status;
            }
        }

        [Authorize]
        [HttpDelete("/api/Admin/Rent/{rentId}")]
        public async Task<ActionResult<HttpStatusCode>> DeleteRent(int rentId)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;

            var response = await _rentAdminService.DeleteRent(rentId, jwtToken, login);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Status;
            }
        }
    }
}
