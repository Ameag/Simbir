using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.Model;
using Simbir.Service.Implementations;
using Simbir.Service.Interfaces;
using System.Net;

namespace Simbir.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController (IPaymentService paymentService)
        {
            _paymentService= paymentService;
        }

        [Authorize]
        [HttpPost("/api/Payment/Hesoyam/{accountId}")]
        public async Task<ActionResult<HttpStatusCode>> Hesoyam()
        {
            var login = User.Identity.Name;
            var response = await _paymentService.Hesoyam(login);
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
