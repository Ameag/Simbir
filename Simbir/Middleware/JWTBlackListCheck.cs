using Simbir.Model;
using Simbir.Repository.Interfaces;

namespace Simbir.Middleware
{
    public class JWTBlackListCheck
    {
        private readonly IBlackListRepository _blackListRepository;
        public JWTBlackListCheck(IBlackListRepository blackListRepository)
        {
            _blackListRepository = blackListRepository;
        }

        public async Task<bool> CheckJWT(string token)
        {
            var blackList = await _blackListRepository.Get(token);
            if(blackList == null)
            {
                return false;
            }
            else
            {
                return blackList.token == token;
            }
            
        }
    }
}
