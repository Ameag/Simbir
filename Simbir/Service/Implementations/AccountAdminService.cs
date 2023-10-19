using Simbir.middleware;
using Simbir.Middleware;
using Simbir.Model;
using Simbir.Repository;
using Simbir.Repository.Interfaces;
using Simbir.Service.Interfaces;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Implementations
{
    public class AccountAdminService : IAccountAdminService
    {
        private readonly PasswordHash passwordHash;
        private readonly IAccountAdminRepository _accountAdminRepository;
        public AccountAdminService(IAccountAdminRepository accountAdminRepository) 
        {
            _accountAdminRepository = accountAdminRepository;
            passwordHash = new PasswordHash();
        }

        public async Task<IBaseResponse<HttpStatusCode>> DeleteAccount(int id)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            try
            {
                var code = await _accountAdminRepository.Delete(id);
                baseResponse.Status = code;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<HttpStatusCode>()
                {
                    Description = $"[SignUp] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<Account>> GetAccount(string id)
        {

            var baseResponse = new BaseResponse<Account>();
            try
            {
                var account = await _accountAdminRepository.Get(id);
                baseResponse.Data = account;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Account>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<HttpStatusCode>> SignUp(AccountAdminInput model)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            var salt = passwordHash.CreateSalt();
            model.password = passwordHash.HashPassword(model.password, salt);
            try
            {
                var code = await _accountAdminRepository.Create(new Account()
                {
                    username = model.username,
                    password = model.password,
                    salt = salt,
                    is_admin = model.is_admin,
                    balance= model.balance
                });
                baseResponse.Status = code;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<HttpStatusCode>()
                {
                    Description = $"[SignUp] : {ex.Message}"
                };
            }
        }
    }
}
