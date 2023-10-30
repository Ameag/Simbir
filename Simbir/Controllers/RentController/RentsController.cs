using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simbir.Model;
using Simbir.Service.Interfaces.RentInterface;
using System.Net;

namespace Simbir.Controllers.RentController
{
    [ApiController]
    public class RentsController : Controller
    {
        private readonly IRentService _rentService;
        public RentsController(IRentService rentService) 
        {
            _rentService = rentService;
        }

        [Authorize]
        [HttpGet("/api/Rent/MyHistory")]
        public async Task<ActionResult<List<Rent>>> GetListRent()
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }
            var response = await _rentService.GetListRent(login, jwtToken);
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
        [HttpGet("/api/Rent/TransportHistory/{transportId}")]
        public async Task<ActionResult<List<Rent>>> GetListTransportHistory(int transportId)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }
            var response = await _rentService.GetListTransportHistory(transportId, login, jwtToken);
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
        [HttpGet("/api/Rent/{rentId}")]
        public async Task<ActionResult<Rent>> GetRent(string id)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }
            var response = await _rentService.GetRent(id, login, jwtToken);
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
        [HttpPost("/api/Rent/New/{transportId}")]
        public async Task<ActionResult<HttpStatusCode>> AddNewRent(int transportId, string price_type)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }
            var response = await _rentService.AddNewRent(transportId, price_type, login, jwtToken);
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
        [HttpPost("/api/Rent/End/{rentId}")]
        public async Task<ActionResult<HttpStatusCode>> AddEndRent(string rentId, int lat, int lon)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }
            var response = await _rentService.AddEndRent(rentId, lat, lon, login, jwtToken);
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
        [HttpGet("/api/Rent/Transport")]
        public async Task<ActionResult<List<Transport>>> SearchTransport(double lat, double lon, double radius, string transport_type)
        {
            var response = await _rentService.SearchTransport(lat, lon, radius, transport_type);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Data;
            }
        }
    }
}
