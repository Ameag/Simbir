using Microsoft.EntityFrameworkCore;
using Simbir.Data;
using Simbir.Data.Interfaces;
using Simbir.Model;
using Simbir.Repository.Interfaces;
using System.Net;

namespace Simbir.Repository
{
    public class BlackListRepository : IBlackListRepository
    {
        private readonly ApplicationDbContext _db;
        public BlackListRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public Task<HttpStatusCode> Create(BlackList entity)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<BlackList> Get(string token)
        {
            return await _db.BlackList.FirstOrDefaultAsync(x => x.token == token);
        }

        public Task<List<BlackList>> Select()
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> Update(BlackList entity)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpStatusCode> SignOut(BlackList entity)
        {
            await _db.BlackList.AddAsync(entity);
            await _db.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

    }
}
