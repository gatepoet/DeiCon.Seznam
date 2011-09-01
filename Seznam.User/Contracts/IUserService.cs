using System;
using System.Collections.Generic;

namespace Seznam.User.Contracts
{
    public interface IUserService : IDisposable
    {
        string CreateUser(string username, string password);
        string Authenticate(string username, string password);
        IEnumerable<string> GetUserIds(IEnumerable<string> username);
    }
}