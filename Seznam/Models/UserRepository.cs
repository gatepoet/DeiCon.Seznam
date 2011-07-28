using System;
using System.Collections.Generic;
using System.Linq;

namespace Seznam.Models
{
    public class UserRepository : IUserRepository
    {
        private static List<User> _users = new List<User>();


        public User GetUser(string username)
        {
            return _users.Single(u => u.Username.Equals(username));
        }

        public void Add(User user)
        {
            _users.Add(user);
        }

        public void Remove(string username)
        {
            _users.Remove(GetUser(username));
        }

        public bool Exists(string username)
        {
            return _users.Where(u => u.Username == username).Count() > 0;
        }
    }
}