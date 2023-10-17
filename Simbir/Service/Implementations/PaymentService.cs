using Simbir.Model;
using Simbir.Repository.Interfaces;
using Simbir.Service.Interfaces;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IBaseResponse<HttpStatusCode>> Hesoyam(string login)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            try
            {
                var account = await _paymentRepository.Get(login);
                if (account == null)
                {
                    return new BaseResponse<HttpStatusCode>()
                    {
                        Description = $"[UpdateAccount] : Аккаунт с логином {login} не существует"
                    };
                }
                account.balance += 250;
                var code = await _paymentRepository.Update(account);
                baseResponse.Status = code;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<HttpStatusCode>()
                {
                    Description = $"[UpdateAccount] : {ex.Message}"
                };
            }
        }
    }
}
