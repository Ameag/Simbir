using Microsoft.EntityFrameworkCore;
using Simbir.Data;
using Simbir.Model;
using Simbir.Repository.Interfaces;
using System.Net;

namespace Simbir.Repository
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

        public Task<List<Account>> Select()
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> Update(Account entity)
        {
            throw new NotImplementedException();
        }
    }
}
