using Microsoft.EntityFrameworkCore;
using Simbir.Data;
using Simbir.Model;
using Simbir.Repository.Interfaces.TransportInterfaces;
using System.Net;

namespace Simbir.Repository.TransportRepository
{
    public class TransportsRepository : ITransportRepository
    {
        private readonly ApplicationDbContext _db;
        public TransportsRepository(ApplicationDbContext db)
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

        public async Task<int> GetIdAccount(string login)
        {
            var account =  await _db.Accounts.FirstOrDefaultAsync(x => x.username == login);
            return account.id;
        }

        public Task<List<Transport>> Select(int start, int count)
        {
            throw new NotImplementedException();
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
    }
}
