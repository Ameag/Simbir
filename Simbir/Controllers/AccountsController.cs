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

        [HttpGet("/api/accounts")] // Добавляем атрибут Route с указанием HTTP-метода и URL-адреса
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            var response = await _accountService.GetAccount();
            var accounts = new List<Account>();
            foreach (var account in response.Data)
            {
                accounts.Add(new Account()
                {
                    login = account.login,
                    password = account.password,
                    is_admin = account.is_admin
                });
            }
            return accounts;
        }

        [HttpPost("/api/accounts/SignUp")]
        public async Task<ActionResult<HttpStatusCode>> SignUp([FromBody] AccountInput model)
        {
            var response = await _accountService.SignUp(model);
            return response.Status;
        }

        [HttpPost("/api/Account/SignIn")]
        public async Task<ActionResult<string>> SignIn([FromBody] AccountInput model)
        {
            var response = await _accountService.SignIn(model);
            return Ok(new { Token = response.Data });
        }
    }
}
