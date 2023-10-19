using Microsoft.EntityFrameworkCore;
using Simbir.Data;
using Simbir.Model;
using Simbir.Repository.Interfaces;
using System.Net;

namespace Simbir.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _db;
        public PaymentRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public Task<HttpStatusCode> Create(Account entity)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> Get(string login)
        {
            return await _db.Accounts.FirstOrDefaultAsync(x => x.username == login);
        }

        public Task<List<Account>> Select()
        {
            throw new NotImplementedException();
        }

        public async Task<HttpStatusCode> Update(Account entity)
        {
            try
            {
                // Связываем объект сущности с контекстом данных
                _db.Accounts.Attach(entity);
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
