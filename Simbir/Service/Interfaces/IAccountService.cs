﻿using Simbir.Model;
using Simbir.Service.Response;

namespace Simbir.Service.Interfaces
{
    public interface IAccountService
    {
        Task<IBaseResponse<IEnumerable<Account>>> GetAccount();
        Task<IBaseResponse<IEnumerable<Account>>> SignUp(AccountInput model);
        Task<IBaseResponse<string>> SignIn(AccountInput accountInput);
    }
}
