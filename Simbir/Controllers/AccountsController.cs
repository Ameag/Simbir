using Microsoft.AspNetCore.Mvc;
using Simbir.Data.Interfaces;
using Simbir.Model;
using Simbir.Service.Interfaces;
using System.Net;

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
        public async Task<ActionResult<List<Account>>> GetAccounts()
        {
            var response = await _accountService.GetAccount();
            var accounts = new List<Account>();
            foreach (var account in response.Data)
            {
                accounts.Add(new Account()
                {
                    id = account.id,
                    login = account.login,
                    password = account.password,
                    is_admin = account.is_admin
                });
            }
            return accounts;
        }

        [HttpPost("/api/accounts/signUp/{login}/{password}")]
        public async Task<ActionResult<HttpStatusCode>> PostAccounts(string login, string password)
        {
            var response = await _accountService.PostAccount(login,password);
            return response.Status;
        }
    }
}
