using Simbir.Middleware;
using Simbir.Model;
using Simbir.Repository.Interfaces;
using Simbir.Repository.Interfaces.AccountInterfaces;
using Simbir.Repository.Interfaces.RentInterfaces;
using Simbir.Repository.Interfaces.TransportInterfaces;
using Simbir.Service.Interfaces.RentInterface;
using Simbir.Service.Response;
using System.Data.Entity.Spatial;
using System.IO;
using System.Net;
using System.Security.Principal;

namespace Simbir.Service.Implementations.RentService
{
    public class RentsService : IRentService
    {
        private readonly IRentRepository _rentRepository;
        private readonly JWTBlackListCheck jWTBlackListCheck;
        private readonly ITransportRepository _transportRepository;
        private readonly CheckTypePrice checkTypePrice;
        private readonly IAccountRepository _accountRepository;
        public RentsService(IRentRepository rentRepository, IBlackListRepository blackListRepository, ITransportRepository transportRepository, IAccountRepository accountRepository) 
        {
            _accountRepository = accountRepository;
            _rentRepository = rentRepository;
            _transportRepository = transportRepository;
            jWTBlackListCheck = new JWTBlackListCheck(blackListRepository);
            checkTypePrice = new CheckTypePrice();
        }

        public async Task<IBaseResponse<HttpStatusCode>> AddEndRent(string rentId, int lan, int lon, string login, string jwtToken)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken)) throw new Exception("Не авторизован");
            try
            {
                var rent = await _rentRepository.Get(rentId);

                if (rent is null) throw new Exception("Нет такой аренды");
                var account = await _accountRepository.Get(rent.user_id.ToString());
                var transport = await _transportRepository.Get(rent.transport_id.ToString());
                var idAccount = await _rentRepository.GetIdAccount(login);
                if (rent.user_id == idAccount)
                {
                    rent.time_end = DateTime.Now.ToUniversalTime();
                    rent.lat = lan;
                    rent.lon = lon;
                    rent.final_price = (int)((rent.time_end - rent.time_start).TotalMinutes * rent.price_of_unit);

                    transport.latitude = lan;
                    transport.longitude = lon;
                    transport.can_be_rented = true;

                    account.balance = account.balance - rent.final_price;

                    await _accountRepository.Update(account);
                    await _transportRepository.Update(transport);
                    var codeRent = await _rentRepository.Update(rent);
                    baseResponse.Status = codeRent;
                    return baseResponse;
                }
                else
                {
                    throw new Exception("Не ты арендовал");
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

        public async Task<IBaseResponse<HttpStatusCode>> AddNewRent(int idTransport, string price_type, string login, string jwtToken)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            try
            {
                if (checkTypePrice.Chek(price_type))
                {
                    var idAccount = await _rentRepository.GetIdAccount(login);
                    var transport = await _transportRepository.Get(idTransport.ToString());
                    if(transport.can_be_rented && transport.owner_id != idAccount) 
                    {
                        switch (price_type)
                        {
                            case "Minutes":
                                {
                                    baseResponse.Status = await CreateRent(idAccount, idTransport, price_type, transport.minute_price);
                                    return baseResponse;
                                }
                            case "Days":
                                {
                                    baseResponse.Status = await CreateRent(idAccount, idTransport, price_type, transport.day_price);
                                    return baseResponse;
                                }   
                            default:
                                throw new Exception("Такого типа аренды нет");
                        }
                    }
                    else
                    {
                        throw new Exception("Транспорт нельзя арендовать");
                    }
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

        public async Task<IBaseResponse<List<Rent>>> GetListRent(string login, string jwtToken)
        {
            var baseResponse = new BaseResponse<List<Rent>>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

            try
            {
                var idAccount = await _rentRepository.GetIdAccount(login);

                var rentHistory = await _rentRepository.SelectHistoryAccount(idAccount);
                baseResponse.Data = rentHistory;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Rent>> ()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<List<Rent>>> GetListTransportHistory(int idTransport, string login, string jwtToken)
        {
            var baseResponse = new BaseResponse<List<Rent>>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

            try
            {
                var idAccount = await _rentRepository.GetIdAccount(login);

                var transportHistory = await _rentRepository.GetListTransportHistory(idTransport,idAccount);
                baseResponse.Data = transportHistory;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Rent>>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<HttpStatusCode> CreateRent(int idAccount, int idTransport, string price_type, int priceOfUnit)
        {
            var code = await _rentRepository.Create(new Rent()
            {
                user_id = idAccount,
                transport_id = idTransport,
                time_start = DateTime.Now.ToUniversalTime(),
                price_of_unit = priceOfUnit,
                price_type = price_type
            });

            var transport = await _transportRepository.Get(idTransport.ToString());
            transport.can_be_rented = false;
            await _transportRepository.Update(transport);

            return code;
        }

        public async Task<IBaseResponse<Rent>> GetRent(string id, string login, string jwtToken)
        {
            var baseResponse = new BaseResponse<Rent>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

            try
            {
                var rent = await _rentRepository.Get(id);
                baseResponse.Data = rent;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Rent>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<List<Rent>>> SearchTransport (double lan, double lon, double radius, string transport_type)
        {
            var baseResponse = new BaseResponse<List<Rent>>();
            try
            {
                var point = DbGeography.FromText($"POINT({lan} {lon})", Convert.ToInt32(radius));
                var transport = _rentRepository.SearchTransport(point, radius);
                baseResponse.Data = null;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Rent>>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }
    }
}
