using Simbir.Data.Interfaces;
using Simbir.Model;
using System.Net;

namespace Simbir.Repository.Interfaces
{
    public interface IBlackListRepository : IBaseRepository<BlackList>
    {
        Task<HttpStatusCode> SignOut(BlackList entity);
    }
}
