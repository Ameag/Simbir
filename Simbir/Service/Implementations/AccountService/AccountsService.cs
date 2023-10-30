using Simbir.Data.Interfaces;
using Simbir.middleware;
using Simbir.Middleware;
using Simbir.Model;
using Simbir.Repository.Interfaces;
using Simbir.Repository.Interfaces.AccountInterfaces;
using Simbir.Service.Interfaces.AccountInterface;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Implementations.AccountService
{
    public class AccountsService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IBlackListRepository _blackListRepository;
        private readonly PasswordHash passwordHash;
        private readonly JWTAuthManager jWTAuthManager;
        private readonly JWTBlackListCheck jWTBlackListCheck;

        public AccountsService(IAccountRepository accountRepository, IConfiguration jwtSettings, IBlackListRepository blackListRepository)
        {
            _accountRepository = accountRepository;
            _blackListRepository = blackListRepository;
            passwordHash = new PasswordHash();
            jWTAuthManager = new JWTAuthManager(jwtSettings);
            jWTBlackListCheck = new JWTBlackListCheck(blackListRepository);
        }

        public async Task<IBaseResponse<Account>> GetAccount(string login, string jwtToken)
        {
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

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
                    username = model.username,
                });
                if (account == null)
                {
                    return new BaseResponse<string>()
                    {
                        Description = "[SignIn] : Аккаунт с логином не существует"
                    };
                }

                if (passwordHash.VerifyPassword(model.password, account.salt, account.password))
                {
                    var token = jWTAuthManager.GenerateJWT(model.username);
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
            catch (Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Description = $"[SignIn] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<HttpStatusCode>> SignOut(string token)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            try
            {
                var code = await _blackListRepository.SignOut(new BlackList() { token = token });
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

        public async Task<IBaseResponse<HttpStatusCode>> SignUp(AccountInput model)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            var salt = passwordHash.CreateSalt();
            model.password = passwordHash.HashPassword(model.password, salt);
            try
            {
                var code = await _accountRepository.Create(new Account()
                {
                    username = model.username,
                    password = model.password,
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

        public async Task<IBaseResponse<HttpStatusCode>> UpdateAccount(string login, AccountInput model, string jwtToken)
        {
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
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

                account.username = model.username;
                account.password = model.password;
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
