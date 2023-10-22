using Microsoft.EntityFrameworkCore;
using Simbir.Data;
using Simbir.Model;
using Simbir.Repository.Interfaces;
using Simbir.Repository.Interfaces.AccountInterfaces;
using System.Net;

namespace Simbir.Repository.AccountRepository.AccountRepository
{
    public class AccountAdminRepository : IAccountAdminRepository
    {
        private readonly ApplicationDbContext _db;
        public AccountAdminRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<HttpStatusCode> Create(Account entity)
        {
            await _db.Accounts.AddAsync(entity);
            await _db.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> Delete(int id)
        {
            var account = await _db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpStatusCode.NotFound;
            }
            _db.Accounts.Remove(account);
            await _db.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        public async Task<Account> Get(string id)
        {
            return await _db.Accounts.FirstOrDefaultAsync(x => x.id == int.Parse(id));
        }

        public async Task<List<Account>> Select(int start, int count)
        {
            // Возвращаем заданное количество элементов
            return await _db.Accounts.OrderBy(x => x.id).Skip(start - 1).Take(count).ToListAsync();
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

        public async Task<bool> СheckAdmin(string login)
        {
            return await _db.Accounts.AnyAsync(x => x.username == login && x.is_admin == true);
        }
    }
}
