using Microsoft.EntityFrameworkCore;
using Simbir.Data;
using Simbir.Model;
using Simbir.Repository.Interfaces.RentInterfaces;
using System.Net;
using System.Security.Principal;

namespace Simbir.Repository.RentRepository
{
    public class RentAdminRepository : IRentAdminRepository
    {
        private readonly ApplicationDbContext _db;
        public RentAdminRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<HttpStatusCode> Create(Rent entity)
        {
            await _db.Rent.AddAsync(entity);
            await _db.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> Delete(int id)
        {
            var rent = await _db.Rent.FindAsync(id);
            if (rent == null)
            {
                return HttpStatusCode.NotFound;
            }
            _db.Rent.Remove(rent);
            await _db.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        public async Task<Rent> Get(string id)
        {
            return await _db.Rent.FirstOrDefaultAsync(x => x.id == int.Parse(id));
        }

        public async Task<List<Rent>> GetListTransportHistory(int transportId)
        {
            return await _db.Rent
            .Where(x => x.transport_id == transportId)
            .OrderBy(x => x.id)
            .ToListAsync();
        }

        public async Task<List<Rent>> GetUserHistory(int userId)
        {
            return await _db.Rent
             .Where(x => x.user_id == userId)
             .OrderBy(x => x.id)
             .ToListAsync();
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

        public async Task<bool> СheckAdmin(string login)
        {
            return await _db.Accounts.AnyAsync(x => x.username == login && x.is_admin == true);
        }
    }
}
