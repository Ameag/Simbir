using Simbir.Data.Interfaces;
using Simbir.middleware;
using Simbir.Middleware;
using Simbir.Model;
using Simbir.Service.Interfaces;
using Simbir.Service.Response;

namespace Simbir.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly PasswordHash passwordHash;
        private readonly JWTAuthManager jWTAuthManager;

        public AccountService(IAccountRepository accountRepository, IConfiguration jwtSettings)
        {
            _accountRepository = accountRepository;
            passwordHash = new PasswordHash();
            jWTAuthManager = new JWTAuthManager(jwtSettings);
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

        public async Task<IBaseResponse<string>> SignIn(AccountInput model)
        {
            var baseResponse = new BaseResponse<string>();
            try
            {
                var account = await _accountRepository.SignIn(new Account()
                {
                    login = model.login,
                });
                var a = passwordHash.PasswordHasher(model.password);
                if (a == account.password)
                {
                    var token = jWTAuthManager.GenerateJWT(model.login);
                    baseResponse.Data = token;
                    return baseResponse;
                }
                else
                {
                    return new BaseResponse<string>()
                    {
                        Description = $"Не верные данные"
                    };
                }
            }
            catch(Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Account>>> SignUp(AccountInput model)
        {
            var baseResponse = new BaseResponse<IEnumerable<Account>>();
            model.password = passwordHash.PasswordHasher(model.password);
            try
            {
                var code = await _accountRepository.Create(new Account() 
                { 
                    login = model.login,
                    password= model.password,
                });
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
