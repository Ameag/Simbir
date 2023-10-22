using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Simbir.Data.Interfaces;
using Simbir.Middleware;
using Simbir.Model;
using Simbir.Service.Interfaces.AccountInterface;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Simbir.Controllers.AccountController
{
    [ApiController] // Добавляем атрибут ApiController
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet("/api/Accounts/Me")] // Добавляем атрибут Route с указанием HTTP-метода и URL-адреса
        public async Task<ActionResult<Account>> GetAccount()
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);

            var login = User.Identity.Name;
            if (login == null)
            {
                return Unauthorized();
            }

            var response = await _accountService.GetAccount(login, jwtToken);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Data;
            }
        }

        [HttpPost("/api/Accounts/SignUp")]//регистрация
        public async Task<ActionResult<HttpStatusCode>> SignUp([FromBody] AccountInput model)
        {
            var response = await _accountService.SignUp(model);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Status;
            }
        }

        [HttpPost("/api/Account/SignIn")]//войти
        public async Task<ActionResult<string>> SignIn([FromBody] AccountInput model)
        {
            var response = await _accountService.SignIn(model);
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
        [HttpPut("/api/Accounts/Update")]
        public async Task<ActionResult<HttpStatusCode>> UpdateAccount([FromBody] AccountInput model)
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            var login = User.Identity.Name;
            if (login == null)
            {
                Unauthorized();
            }

            var response = await _accountService.UpdateAccount(login, model, jwtToken);
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
        [HttpPost("/api/Account/SignOut")]
        public async Task<ActionResult<HttpStatusCode>> Logout()
        {
            var authHeader = Request.Headers["Authorization"];
            var jwtToken = authHeader.ToString().Substring("bearer ".Length);
            if (jwtToken == null)
            {
                return Unauthorized();
            }

            var response = await _accountService.SignOut(jwtToken);
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
