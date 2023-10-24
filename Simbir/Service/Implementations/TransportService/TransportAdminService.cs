using Simbir.Middleware;
using Simbir.Model;
using Simbir.Repository.Interfaces;
using Simbir.Repository.Interfaces.TransportInterfaces;
using Simbir.Repository.TransportRepository;
using Simbir.Service.Interfaces.TransportInterface;
using Simbir.Service.Response;
using System.Net;

namespace Simbir.Service.Implementations.TransportService
{
    public class TransportAdminService : ITransportAdminService
    {
        private readonly ITransportAdminRepository _transporstAdminRepository;
        private readonly JWTBlackListCheck jWTBlackListCheck;
        private readonly CheckTypeTransport checkTypeTransport;
        public TransportAdminService(ITransportAdminRepository transporstAdminRepository, IBlackListRepository blackListRepository) 
        {
            _transporstAdminRepository = transporstAdminRepository;
            jWTBlackListCheck = new JWTBlackListCheck(blackListRepository);
            checkTypeTransport = new CheckTypeTransport();
        }
        public async Task<IBaseResponse<HttpStatusCode>> AddTransport(TransportInputAdmin model, string jwtToken, string login)
        {
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            var baseResponse = new BaseResponse<HttpStatusCode>();
            try
            {
                if (await _transporstAdminRepository.GetIdAccount(model.owner_id))
                {
                    throw new Exception("аккаунт не найден");
                }

                if (await _transporstAdminRepository.СheckAdmin(login) && checkTypeTransport.Chek(model.transport_type))
                {
                    var code = await _transporstAdminRepository.Create(new Transport()
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
                        owner_id = model.owner_id

                    });
                    baseResponse.Status = code;
                    return baseResponse;
                }
                else
                {
                    return new BaseResponse<HttpStatusCode>()
                    {
                        Description = $"Аккаунт не явлеяется админом либо неправильный тип транспорта"
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

        public async Task<IBaseResponse<HttpStatusCode>> DeleteTransport(int id, string jwtToken, string login)
        {
            var baseResponse = new BaseResponse<HttpStatusCode>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            try
            {
                if (await _transporstAdminRepository.СheckAdmin(login))
                {
                    var code = await _transporstAdminRepository.Delete(id);
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

        public async  Task<IBaseResponse<List<Transport>>> GetListTransport(string login, string jwtToken, int start, int count, string type)
        {
            var baseResponse = new BaseResponse<List<Transport>>();
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

            try
            {
                if (await _transporstAdminRepository.СheckAdmin(login))
                {
                    var transports = await _transporstAdminRepository.SelectOfType(start, count, type);
                    baseResponse.Data = transports;
                    return baseResponse;
                }
                else
                {
                    return new BaseResponse<List<Transport>>()
                    {
                        Description = $"Аккаунт не явлеяется админом"
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Transport>>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<Transport>> GetTransport(string id, string jwtToken, string login)
        {
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }

            var baseResponse = new BaseResponse<Transport>();
            try
            {
                if (await _transporstAdminRepository.СheckAdmin(login))
                {
                    var transport = await _transporstAdminRepository.Get(id);
                    baseResponse.Data = transport;
                    return baseResponse;
                }
                else
                {
                    return new BaseResponse<Transport>()
                    {
                        Description = $"Аккаунт не явлеяется админом"
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<Transport>()
                {
                    Description = $"[GetAccount] : {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<HttpStatusCode>> UpdateTransport(string id, TransportInputAdmin model, string jwtToken, string login)
        {
            if (await jWTBlackListCheck.CheckJWT(jwtToken))
            {
                throw new Exception("Не авторизован");
            }
            var baseResponse = new BaseResponse<HttpStatusCode>();
            try
            {
  
                var transport = await _transporstAdminRepository.Get(id);


                if (checkTypeTransport.Chek(model.transport_type) && await _transporstAdminRepository.СheckAdmin(login))
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
                    transport.owner_id= model.owner_id;
                    transport.transport_type = model.transport_type;

                    var code = await _transporstAdminRepository.Update(transport);
                    baseResponse.Status = code;
                    return baseResponse;
                }
                else
                {
                    throw new Exception("Неверный тип транспорта или аккаунт не является админом");
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
