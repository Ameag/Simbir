using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.Model;
using Simbir.Service.Implementations;
using Simbir.Service.Interfaces.AccountInterface;
using System.Net;

namespace Simbir.Controllers.AccountController
{
    [ApiController]
    public class AccountsAdminController : Controller
    {
        private readonly IAccountAdminService _accountAdminService;
        public AccountsAdminController(IAccountAdminService accountAdminService)
        {
            _accountAdminService = accountAdminService;
        }

        [Authorize]
        [HttpPost("/api/Admin/Account")]//регистрация
        public async Task<ActionResult<HttpStatusCode>> SignUp([FromBody] AccountAdminInput model)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);

            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var response = await _accountAdminService.SignUp(model, login, jwtToken);
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
        [HttpGet("/api/Admin/Account/{id}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var response = await _accountAdminService.GetAccount(id, login, jwtToken);
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
        [HttpDelete("/api/Admin/Account/{id}")]
        public async Task<ActionResult<HttpStatusCode>> DeleteAccount(int id)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var response = await _accountAdminService.DeleteAccount(id, login, jwtToken);
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
        [HttpPut("/api/Admin/Account/{id}")]
        public async Task<ActionResult<HttpStatusCode>> UpdateAccount (string id, [FromBody] AccountAdminInput model)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var response = await _accountAdminService.UpdateAccount(id, login, jwtToken, model);
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
        [HttpGet("/api/Admin/Account")]
        public async Task<ActionResult<List<Account>>> GetListAccount (int start, int count)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }
            var response = await _accountAdminService.GetListAccount(login, jwtToken, start, count);
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
