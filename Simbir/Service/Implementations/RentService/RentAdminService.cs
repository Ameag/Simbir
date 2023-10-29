using Simbir.Middleware;
using Simbir.Model;
using Simbir.Repository.Interfaces.RentInterfaces;
using Simbir.Repository.Interfaces;
using Simbir.Service.Interfaces.RentInterface;
using Simbir.Service.Response;
using Simbir.Repository.Interfaces.TransportInterfaces;
using Microsoft.AspNetCore.Mvc;
using Simbir.Repository.RentRepository;
using System.Net;
using Simbir.Repository.TransportRepository;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using Simbir.middleware;
using Simbir.Repository.Interfaces.AccountInterfaces;

namespace Simbir.Service.Implementations.RentService
{
    public class RentAdminService : IRentAdminService
    {
        private readonly CheckTypePrice checkTypePrice;
        private readonly JWTBlackListCheck jWTBlackListCheck;
        private readonly ITransportRepository _transportRepository;
        private readonly IRentAdminRepository _rentAdminRepository;
        private readonly IAccountRepository _accountRepository;
        public RentAdminService(IRentAdminRepository rentAdminRepository, IBlackListRepository blackListRepository, ITransportRepository transportRepository, IAccountRepository accountRepository) 
        {
            _accountRepository = accountRepository;
            _rentAdminRepository = rentAdminRepository;
            _transportRepository = transportRepository;
            jWTBlackListCheck = new JWTBlackListCheck(blackListRepository);
            checkTypePrice = new CheckTypePrice();
        }

        public async Task<IBaseResponse<HttpStatusCode>> AddEndRent(int rentId, string jwtToken, double lat, double lon, string login)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            try
            {
                if (await _rentAdminRepository.СheckAdmin(login))
                {
                    var rent = await _rentAdminRepository.Get(rentId.ToString());
                    if (rent is null) throw new Exception("Нет такой аренды");
                    var account = await _accountRepository.Get(rent.user_id.ToString());
                    var transport = await _transportRepository.Get(rent.transport_id.ToString());
                    transport.can_be_rented = true;

                    rent.lon = lon;
                    rent.lat = lat;
                    account.balance = account.balance - rent.final_price;

                    await _accountRepository.Update(account);
                    await _transportRepository.Update(transport);
                    var code = await _rentAdminRepository.Update(rent);
                    baseResponse.Status = code;
                    return baseResponse;
                }
                else
                {
                    throw new Exception("Не админ");
                }
                
            }
            catch (Exception ex)
            {
                return new BaseResponse<HttpStatusCode>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<HttpStatusCode>> AddNewRent([FromBody] InputAdminRent model, string jwtToken, string login)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            if (model is null) throw new Exception("Данные невернно введены");
            try
            {
                if (checkTypePrice.Chek(model.price_type) && await _rentAdminRepository.СheckAdmin(login))
                {
                    var transport = await _transportRepository.Get(model.transport_id.ToString());
                    if (transport is null) throw new Exception("Транспорт не найден");

                    var code = await _rentAdminRepository.Create(new Rent()
                    {
                        user_id = model.user_id,
                        transport_id = model.transport_id,
                        time_start = model.time_start,
                        time_end = model.time_end,
                        price_of_unit = model.price_of_unit,
                        price_type = model.price_type,
                        final_price= model.final_price,
                    });
              
                    transport.can_be_rented = false;
                    await _transportRepository.Update(transport);
                    baseResponse.Status = code;
                    return baseResponse;
                }
                else
                {
                    throw new Exception("Такого типа аренды нет");
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<HttpStatusCode>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<HttpStatusCode>> DeleteRent(int id, string jwtToken, string login)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            try
            {
                if (await _rentAdminRepository.СheckAdmin(login))
                {
                    var code = await _rentAdminRepository.Delete(id);
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

        public async Task<IBaseResponse<Rent>> GetRent(string id, string jwtToken, string login)
        {
            var baseResponse = new BaseResponse<Rent>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

            try
            {
                if (await _rentAdminRepository.СheckAdmin(login))
                {
                    var rent = await _rentAdminRepository.Get(id);
                    baseResponse.Data = rent;
                    return baseResponse;
                }
                else
                {
                    throw new Exception("Не админ");
                }
               
            }
            catch (Exception ex)
            {
                return new BaseResponse<Rent>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<List<Rent>>> GetTransportHistory(int transportId, string jwtToken, string login)
        {
            var baseResponse = new BaseResponse<List<Rent>>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

            try
            {
                if (await _rentAdminRepository.СheckAdmin(login))
                {
                    var transportHistory = await _rentAdminRepository.GetListTransportHistory(transportId);
                    baseResponse.Data = transportHistory;
                    return baseResponse;
                }
                else
                {
                    throw new Exception("Не админ");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Rent>>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<List<Rent>>> GetUserHistory(int userId, string jwtToken, string login)
        {
            var baseResponse = new BaseResponse<List<Rent>>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

            try
            {
                if (await _rentAdminRepository.СheckAdmin(login))
                {
                    var rentHistory = await _rentAdminRepository.GetUserHistory(userId);
                    baseResponse.Data = rentHistory;
                    return baseResponse;
                }
                else
                {
                    throw new Exception("Не админ");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Rent>>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<HttpStatusCode>> UpdateRent(int id, [FromBody] InputAdminRent model,string jwtToken, string login)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            if (model is null) throw new Exception("Данные невернно введены");
            try
            {
                if (await _rentAdminRepository.СheckAdmin(login) && checkTypePrice.Chek(model.price_type))
                {
                    var rent = await _rentAdminRepository.Get(id.ToString());
                    if (rent == null)
                    {
                        return new BaseResponse<HttpStatusCode>()
                        {
                            Description = $"[UpdateRent] : Рент с логином {id} не существует"
                        };
                    }

                    rent.transport_id = model.transport_id;
                    rent.user_id = model.user_id;
                    rent.time_start = model.time_start;
                    rent.time_end = model.time_end;
                    rent.price_of_unit = model.price_of_unit;
                    rent.price_type = model.price_type;
                    rent.final_price = model.final_price;

                    var code = await _rentAdminRepository.Update(rent);
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
            catch(Exception ex) 
            {
                return new BaseResponse<HttpStatusCode>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }
    }
}
