using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.Model;
using Simbir.Service.Interfaces.AccountInterface;
using Simbir.Service.Interfaces.TransportInterface;
using System.Net;

namespace Simbir.Controllers.TransportController
{
    [ApiController]
    public class TransportsController : Controller
    {
        private readonly ITransportService _transportService;
        public TransportsController(ITransportService transportService) 
        {
            _transportService = transportService;
        }

        [Authorize]
        [HttpPost("/api/Transport")]
        public async Task<ActionResult<HttpStatusCode>> AddTransport([FromBody] TransportInput model)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }
            var response = await _transportService.AddTransport(model, jwtToken, login);
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
        [HttpGet("/api/Transport/{id}")]
        public async Task<ActionResult<Transport>> GetTransport(string id)
        {
            var response = await _transportService.GetTransport(id);
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
        [HttpPut("/api/Transport/{id}")]
        public async Task<ActionResult<HttpStatusCode>> UpdateTransport(string id, [FromBody] TransportInput model)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var response = await _transportService.UpdateTransport(model, id, jwtToken, login);
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
        [HttpDelete("/api/Transport/{id}")]
        public async Task<ActionResult<HttpStatusCode>> DeleteTransport(int id)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var response = await _transportService.DeleteTransport(id, jwtToken, login);
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
