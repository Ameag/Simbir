using Simbir.middleware;
using Simbir.Middleware;
using Simbir.Model;
using Simbir.Repository;
using Simbir.Repository.AccountRepository;
using Simbir.Repository.Interfaces;
using Simbir.Repository.Interfaces.AccountInterfaces;
using Simbir.Service.Interfaces.AccountInterface;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Implementations.AccountService
{
    public class AccountAdminService : IAccountAdminService
    {
        private readonly PasswordHash passwordHash;
        private readonly IAccountAdminRepository _accountAdminRepository;
        private readonly JWTBlackListCheck jWTBlackListCheck;
        public AccountAdminService(IAccountAdminRepository accountAdminRepository, IBlackListRepository blackListRepository)
        {
            _accountAdminRepository = accountAdminRepository;
            passwordHash = new PasswordHash();
            jWTBlackListCheck = new JWTBlackListCheck(blackListRepository);
        }

        public async Task<IBaseResponse<HttpStatusCode>> DeleteAccount(int id, string login, string jwtToken)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            try
            {
                if (await _accountAdminRepository.СheckAdmin(login))
                {
                    var code = await _accountAdminRepository.Delete(id);
                    baseResponse.Status = code;
                    return baseResponse;
                }
                else
                {
                    return new BaseResponse<HttpStatusCode>()
                    {
                        Description = $"Аккаунт не явлеяется админом"
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<HttpStatusCode>()
                {
                    Description = $"[SignUp] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<Account>> GetAccount(string id, string login, string jwtToken)
        {
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

            var baseResponse = new BaseResponse<Account>();
            try
            {
                if (await _accountAdminRepository.СheckAdmin(login))
                {
                    var account = await _accountAdminRepository.Get(id);
                    baseResponse.Data = account;
                    return baseResponse;
                }
                else
                {
                    return new BaseResponse<Account>()
                    {
                        Description = $"Аккаунт не явлеяется админом"
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<Account>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<HttpStatusCode>> SignUp(AccountAdminInput model, string login, string jwtToken)
        {
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

            var baseResponse = new BaseResponse<HttpStatusCode>();
            var salt = passwordHash.CreateSalt();
            model.password = passwordHash.HashPassword(model.password, salt);
            try
            {
                if(await _accountAdminRepository.СheckAdmin(login))
                {
                    var code = await _accountAdminRepository.Create(new Account()
                    {
                        username = model.username,
                        password = model.password,
                        salt = salt,
                        is_admin = model.is_admin,
                        balance = model.balance
                    });
                    baseResponse.Status = code;
                    return baseResponse;
                }
                else
                {
                    return new BaseResponse<HttpStatusCode>()
                    {
                        Description = $"Аккаунт не явлеяется админом"
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<HttpStatusCode>()
                {
                    Description = $"[SignUp] : {ex.Message}"
                };
            }
        }
         
        public async Task<IBaseResponse<HttpStatusCode>> UpdateAccount(string id, string login, string jwtToken, AccountAdminInput model)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            try
            {
                if (await _accountAdminRepository.СheckAdmin(login))
                {
                    var account = await _accountAdminRepository.Get(id);
                    if (account == null)
                    {
                        return new BaseResponse<HttpStatusCode>()
                        {
                            Description = $"[UpdateAccount] : Аккаунт с логином {id} не существует"
                        };
                    }
                    model.password = passwordHash.HashPassword(model.password, account.salt);

                    account.username = model.username;
                    account.password = model.password;
                    account.is_admin = model.is_admin;
                    account.balance = model.balance;
                    var code = await _accountAdminRepository.Update(account);
                    baseResponse.Status = code;
                    return baseResponse;
                }
                else
                {
                    return new BaseResponse<HttpStatusCode>()
                    {
                        Description = $"Аккаунт не явлеяется админом"
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<HttpStatusCode>()
                {
                    Description = $"[SignUp] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<List<Account>>> GetListAccount(string login, string jwtToken, int start, int count)
        {
            var baseResponse = new BaseResponse<List<Account>>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

            try
            {
                if (await _accountAdminRepository.СheckAdmin(login))
                {
                    var account = await _accountAdminRepository.Select(start, count); 
                    baseResponse.Data = account;
                    return baseResponse;
                }
                else
                {
                    return new BaseResponse<List<Account>>()
                    {
                        Description = $"Аккаунт не явлеяется админом"
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Account>>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }

        }
    }
}
