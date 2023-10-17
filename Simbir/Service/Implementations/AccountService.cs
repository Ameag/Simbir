using Simbir.Data.Interfaces;
using Simbir.middleware;
using Simbir.Middleware;
using Simbir.Model;
using Simbir.Service.Interfaces;
using Simbir.Service.Response;
using System.Net;

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

        public async Task<IBaseResponse<Account>> GetAccount(string login)
        {
            var baseResponse = new BaseResponse<Account>();
            try
            {
                var account = await _accountRepository.Get(login);
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

        public async Task<IBaseResponse<string>> SignIn(AccountInput model)
        {
            var baseResponse = new BaseResponse<string>();
            try
            {
                var account = await _accountRepository.SignIn(new Account()
                {
                    username = model.login,
                });
                if (account == null)
                {
                    return new BaseResponse<string>()
                    {
                        Description = "[UpdateAccount] : Аккаунт с логином не существует"
                    };
                }

                if (passwordHash.VerifyPassword(model.password, account.salt, account.password))
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
                    Description = $"[SignIn] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<HttpStatusCode>> SignUp(AccountInput model)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            var salt = passwordHash.CreateSalt();
            model.password = passwordHash.HashPassword(model.password, salt);
            try
            {
                var code = await _accountRepository.Create(new Account() 
                { 
                    username = model.login,
                    password= model.password,
                    salt = salt
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

        public async Task<IBaseResponse<HttpStatusCode>> UpdateAccount(string login, AccountInput model)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            try
            {
                var account = await _accountRepository.Get(login);
                if (account == null)
                {
                    return new BaseResponse<HttpStatusCode>()
                    {
                        Description = $"[UpdateAccount] : Аккаунт с логином {login} не существует"
                    };
                }
                model.password = passwordHash.HashPassword(model.password, account.salt);

                account.username= model.login;
                account.password= model.password;
                var code = await _accountRepository.Update(account);
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
