using Simbir.middleware;
using Simbir.Middleware;
using Simbir.Model;
using Simbir.Repository.AccountRepository;
using Simbir.Repository.AccountRepository.AccountRepository;
using Simbir.Repository.Interfaces;
using Simbir.Repository.Interfaces.AccountInterfaces;
using Simbir.Repository.Interfaces.TransportInterfaces;
using Simbir.Service.Interfaces.TransportInterface;
using Simbir.Service.Response;
using System.Data.Entity.Spatial;
using System.Drawing;
using System.Net;

namespace Simbir.Service.Implementations.TransportService
{
    public class TransportsService : ITransportService
    {
        private readonly ITransportRepository _transporstRepository;
        private readonly JWTBlackListCheck jWTBlackListCheck;
        private readonly CheckTypeTransport checkTypeTransport;

        public TransportsService(ITransportRepository transporstRepository, IBlackListRepository blackListRepository)
        {
            _transporstRepository = transporstRepository;
            jWTBlackListCheck = new JWTBlackListCheck(blackListRepository);
            checkTypeTransport = new CheckTypeTransport();
        }

        public async Task<IBaseResponse<HttpStatusCode>> AddTransport(TransportInput model, string jwtToken, string login)
        {
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            var baseResponse = new BaseResponse<HttpStatusCode>();
            try
            {
                var account = await _transporstRepository.GetIdAccount(login);
                if(account == null)
                {
                    throw new Exception("аккаунт не найден");
                }


                if (checkTypeTransport.Chek(model.transport_type))
                {
                    var code = await _transporstRepository.Create(new Transport()
                    {
                        transport_type = model.transport_type,
                        identifier = model.identifier,
                        can_be_rented = model.can_be_rented,
                        model = model.model,
                        color = model.color,
                        description = model.description,
                        latitude = model.latitude,
                        longitude = model.longitude,
                        minute_price = model.minute_price,
                        day_price = model.day_price,
                        owner_id = account,
                    });
                    baseResponse.Status = code;
                    return baseResponse;
                }
                else
                {
                    throw new Exception("Неверный тип транспорта");
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

        public async Task<IBaseResponse<HttpStatusCode>> DeleteTransport(int id, string jwtToken, string login)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            try
            {
                var account = await _transporstRepository.GetIdAccount(login);
                if (account == null)
                {
                    throw new Exception("аккаунт не найден");
                }
                var transport = await _transporstRepository.Get(id.ToString());

                if (transport.owner_id != account)
                {
                    throw new Exception("не твой транспорт");
                }

                  var code = await _transporstRepository.Delete(id);
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

        public async Task<IBaseResponse<Transport>> GetTransport(string id)
        {
            var baseResponse = new BaseResponse<Transport>();
            try
            {
                var transport = await _transporstRepository.Get(id);
                baseResponse.Data = transport;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Transport>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<HttpStatusCode>> UpdateTransport(TransportInput model, string id, string jwtToken, string login)
        {
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            var baseResponse = new BaseResponse<HttpStatusCode>();
            try
            {
                var account = await _transporstRepository.GetIdAccount(login);
                if (account == null)
                {
                    throw new Exception("аккаунт не найден");
                }
                var transport = await _transporstRepository.Get(id);

                if (transport.owner_id != account)
                {
                    throw new Exception("не твой транспорт");
                }

                if (checkTypeTransport.Chek(model.transport_type))
                {
                    transport.identifier = model.identifier;
                    transport.can_be_rented = model.can_be_rented;
                    transport.model = model.model;
                    transport.color = model.color;
                    transport.description = model.description;
                    transport.latitude = model.latitude;
                    transport.longitude = model.longitude;
                    transport.minute_price = model.minute_price;
                    transport.day_price = model.day_price;

                    var code = await _transporstRepository.Update(transport);
                    baseResponse.Status = code;
                    return baseResponse;
                }
                else
                {
                    throw new Exception("Неверный тип транспорта");
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
    }
}
