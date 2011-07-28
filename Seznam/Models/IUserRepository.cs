using System;
using System.Web.Security;

namespace Seznam.Models
{
    public interface IUserRepository
    {
        User GetUser(string username);
        void Add(User user);
        void Remove(string username);
        bool Exists(string username);
    }
}