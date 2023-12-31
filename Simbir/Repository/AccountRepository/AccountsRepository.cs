﻿using Microsoft.EntityFrameworkCore;
using Simbir.Data;
using Simbir.Data.Interfaces;
using Simbir.Model;
using Simbir.Repository.Interfaces;
using Simbir.Repository.Interfaces.AccountInterfaces;
using System.Net;

namespace Simbir.Repository.AccountRepository
{
    public class AccountsRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        public AccountsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<HttpStatusCode> Create(Account entity)
        {
            await _db.Accounts.AddAsync(entity);
            await _db.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

        public Task<HttpStatusCode> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> Get(string login)
        {
            return await _db.Accounts.FirstOrDefaultAsync(x => x.username == login);
        }


        public Task<List<Account>> Select(int start, int count)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> SignIn(Account entity)
        {
            return await _db.Accounts.FirstOrDefaultAsync(a => a.username == entity.username);
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
