using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.Model;
using Simbir.Service.Implementations;
using Simbir.Service.Interfaces;
using System.Net;

namespace Simbir.Controllers
{
    [ApiController]
    public class AccountsAdminController : Controller
    {
        private readonly IAccountAdminService _accountAdminService;
        public AccountsAdminController(IAccountAdminService accountAdminService)
        {
            _accountAdminService = accountAdminService;
        }

        [HttpPost("/api/Admin/Account")]//регистрация
        public async Task<ActionResult<HttpStatusCode>> SignUp([FromBody] AccountAdminInput model)
        {
            var response = await _accountAdminService.SignUp(model);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Status;
            }
        }

        [HttpGet("/api/Admin/Account/{id}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {

            var response = await _accountAdminService.GetAccount(id);
            if (response.Description != null)
            {
                throw new Exception(response.Description);
            }
            else
            {
                return response.Data;
            }
        }
        [HttpDelete("/api/Admin/Account/{id}")]
        public async Task<ActionResult<HttpStatusCode>> DeleteAccount(int id)
        {
            var response = await _accountAdminService.DeleteAccount(id);
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
