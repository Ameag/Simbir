using Simbir.Data.Interfaces;
using Simbir.middleware;
using Simbir.Model;
using Simbir.Service.Interfaces;
using Simbir.Service.Response;

namespace Simbir.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IBaseResponse<IEnumerable<Account>>> GetAccount()
        {
            var baseResponse = new BaseResponse<IEnumerable<Account>>();
            try
            {
                var account = await _accountRepository.Select();
                baseResponse.Data= account;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Account>>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Account>>> PostAccount(string login, string password)
        {
            var baseResponse = new BaseResponse<IEnumerable<Account>>();
            var accounts = new Account();
            password = PasswordHasher.HashPassword(password);
            accounts.login = login;
            accounts.password = password;
            try
            {
                var code = await _accountRepository.Create(accounts);
                baseResponse.Status = code;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Account>>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }
    }
}
