using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.Model;
using Simbir.Service.Implementations.TransportService;
using Simbir.Service.Interfaces.TransportInterface;
using System.Net;

namespace Simbir.Controllers.TransportController
{
    [ApiController]
    public class TransportAdminController : Controller
    {
        private readonly ITransportAdminService _transportAdminService;
        public TransportAdminController(ITransportAdminService transportAdminService)
        {
            _transportAdminService = transportAdminService;
        }
        [Authorize]
        [HttpPost("/api/Admin/Transport")]
        public async Task<ActionResult<HttpStatusCode>> AddTransport([FromBody] TransportInputAdmin model)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }
            var response = await _transportAdminService.AddTransport(model, jwtToken, login);
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
        [HttpGet("/api/Admin/Transport/{id}")]
        public async Task<ActionResult<Transport>> GetTransport(string id)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            var response = await _transportAdminService.GetTransport(id, jwtToken, login);
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
        [HttpDelete("/api/Admin/Transport/{id}")]

        public async Task<ActionResult<HttpStatusCode>> DeleteTransport(int id)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var response = await _transportAdminService.DeleteTransport(id, jwtToken, login);
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
        [HttpGet("/api/Admin/Transport")]
        public async Task<ActionResult<List<Transport>>> GetListTransport(int start, int count, string type)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }
            var response = await _transportAdminService.GetListTransport(login, jwtToken, start, count, type);
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
        [HttpPut("/api/Admin/Transport/{id}")]
        public async Task<ActionResult<HttpStatusCode>> UpdateTransport([FromBody] TransportInputAdmin model, string id)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var response = await _transportAdminService.UpdateTransport(id, model, jwtToken, login);
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
