using Microsoft.EntityFrameworkCore;
using Simbir.Data;
using Simbir.Model;
using Simbir.Repository.Interfaces.RentInterfaces;
using System.Data.Entity.Spatial;
using System.Net;
using System.Security.Principal;

namespace Simbir.Repository.RentRepository
{
    public class RentsRepository : IRentRepository
    {
        private readonly ApplicationDbContext _db;
        public RentsRepository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public async Task<HttpStatusCode> Create(Rent entity)
        {
            await _db.Rent.AddAsync(entity);
            await _db.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

        public Task<HttpStatusCode> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Rent> Get(string id)
        {
            return await _db.Rent.FirstOrDefaultAsync(x => x.id == int.Parse(id));
        }

        public Task<List<Rent>> Select(int start, int count)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpStatusCode> Update(Rent entity)
        {
            try
            {
                // Связываем объект сущности с контекстом данных
                _db.Rent.Attach(entity);
                // Указываем, что аккаунт изменен
                _db.Entry(entity).State = EntityState.Modified;
                // Сохраняем изменения в базе данных
                await _db.SaveChangesAsync();
                // Возвращаем успешный статус
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                // Возвращаем ошибку с сообщением исключения
                throw ex;
            }
        }

        public async Task<int> GetIdAccount(string login)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(x => x.username == login);
            if (account == null)
            {
                throw new Exception("аккаунт не найден");
            }
            return account.id;
        }

        public async Task<List<Rent>> SelectHistoryAccount(int idAccount)
        {
            return await _db.Rent
             .Where(x => x.user_id == idAccount)
             .OrderBy(x => x.id)
             .ToListAsync();
        }

        public async Task<List<Rent>> GetListTransportHistory(int idTransport, int idAccount)
        {
            return await _db.Rent
             .Where(x => x.user_id == idAccount && x.transport_id == idTransport)
             .OrderBy(x => x.id)
             .ToListAsync();
        }

        public async  Task<List<Rent>> SearchTransport(DbGeography point, double radius)
        {
            return await _db.Rent
             .ToListAsync();

        }
        public async Task<List<Transport>> GetAllTransportType(string transport_type)
        {
            return await _db.Transport
                .Where(x => x.transport_type == transport_type)
                .ToListAsync();

        }
        public async Task<List<Transport>> GetAllTransport(string transport_type)
        {
            return await _db.Transport.ToListAsync();

        }
    }
}
