using Microsoft.EntityFrameworkCore;
using Simbir.Data.Interfaces;
using Simbir.Model;
using System.Net;

namespace Simbir.Data.Repository
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

        public async Task<Account> Get(int id)
        {
            return await _db.Accounts.FirstOrDefaultAsync(x => x.id == id);
        }

        public Account GetAccount(int id) 
        {
            throw new NotImplementedException();
        }

        public async Task<List<Account>> Select()
        {
            return await _db.Accounts.ToListAsync();
        }

    }
}
