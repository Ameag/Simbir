﻿using System.Net;

namespace Simbir.Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<HttpStatusCode> Create(T entity);
        Task<T> Get(int id);
        Task<List<T>> Select();
        Task<bool> Delete(T entity);
    }
}
