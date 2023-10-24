using Microsoft.EntityFrameworkCore;
using Simbir.Data;
using Simbir.Model;
using Simbir.Repository.Interfaces.TransportInterfaces;
using System.Net;

namespace Simbir.Repository.TransportRepository
{
    public class TransportAdminRepository : ITransportAdminRepository
    {
        private readonly ApplicationDbContext _db;
        public TransportAdminRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<HttpStatusCode> Create(Transport entity)
        {
            await _db.Transport.AddAsync(entity);
            await _db.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> Delete(int id)
        {
            var transport = await _db.Transport.FindAsync(id);
            if (transport == null)
            {
                return HttpStatusCode.NotFound;
            }
            _db.Transport.Remove(transport);
            await _db.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        public async Task<Transport> Get(string id)
        {
            return await _db.Transport.FirstOrDefaultAsync(x => x.id == int.Parse(id));
        }

        public async Task<List<Transport>> Select(int start, int count)
        {
            return await _db.Transport.OrderBy(x => x.id).Skip(start - 1).Take(count).ToListAsync();
        }

        public async Task<HttpStatusCode> Update(Transport entity)
        {
            try
            {
                // Связываем объект сущности с контекстом данных
                _db.Transport.Attach(entity);
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

        public async Task<bool> СheckAdmin(string login)
        {
            return await _db.Accounts.AnyAsync(x => x.username == login && x.is_admin == true);
        }

        public async Task<bool> GetIdAccount(int id)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(x => x.id == id);
            if(account == null)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Transport>> SelectOfType(int start, int count, string type)
        {
            return await _db.Transport
               .Where(x => x.transport_type == type) 
               .OrderBy(x => x.id) 
               .Skip(start - 1) 
               .Take(count) 
               .ToListAsync();
        }
    }
}
