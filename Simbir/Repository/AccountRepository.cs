using Microsoft.EntityFrameworkCore;
using Simbir.Data.Interfaces;
using Simbir.Model;
using System.Net;

namespace Simbir.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        public AccountRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<HttpStatusCode> Create(Account entity)
        {
            await _db.Accounts.AddAsync(entity);
            await _db.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

        public async Task<bool> Delete(Account entity)
        {
            _db.Accounts.Remove(entity);
            _db.SaveChangesAsync();

            return true;
        }

        public async Task<Account> Get(string login)
        {
            return await _db.Accounts.FirstOrDefaultAsync(x => x.login == login);
        }

        public async Task<List<Account>> Select()
        {
            return await _db.Accounts.ToListAsync();
        }

        public async Task<Account> SignIn(Account entity)
        {
            return await _db.Accounts.FirstOrDefaultAsync(a => a.login == entity.login);
        }
    }
}
