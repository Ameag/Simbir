using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Simbir.Data.Interfaces;
using Simbir.Model;
using Simbir.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Simbir.Controllers
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
            var login = User.Identity.Name;
            var response = await _accountService.GetAccount(login);
            if(response.Description != null) 
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Data;
            }
        }

        [HttpPost("/api/Accounts/SignUp")]
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

        [HttpPost("/api/Account/SignIn")]
        public async Task<ActionResult<string>> SignIn([FromBody] AccountInput model)
        {
            var response = await _accountService.SignIn(model);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return  response.Data;
            }
        }

        [Authorize]
        [HttpPut("/api/Accounts/Update")] 
        public async Task<ActionResult<HttpStatusCode>> UpdateAccount([FromBody] AccountInput model)
        {
            var login = User.Identity.Name;
            var response = await _accountService.UpdateAccount(login, model);
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
