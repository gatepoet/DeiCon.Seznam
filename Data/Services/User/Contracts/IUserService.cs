using System.Collections.Generic;

namespace Seznam.Data.Services.User.Contracts
{
    public interface IUserService
    {
        string CreateUser(string username, string password);
        string Authenticate(string username, string password);
        IEnumerable<string> GetUserIds(IEnumerable<string> username);
    }
}