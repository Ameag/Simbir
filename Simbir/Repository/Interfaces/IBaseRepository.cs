using System.Net;

namespace Simbir.Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<HttpStatusCode> Create(T entity);
        Task<T> Get(string id);
        Task<List<T>> Select();
        Task<HttpStatusCode> Delete(int id);
        Task<HttpStatusCode> Update(T entity);
    }
}
