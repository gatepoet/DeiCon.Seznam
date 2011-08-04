using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Authentication;

namespace Seznam.Models
{
    public class User
    {
        private readonly string _password;
        private readonly Dictionary<string, SeznamList> _personalLists;
        private readonly Dictionary<string, SeznamList> _sharedLists;
        public IEnumerable<SeznamList> PersonalLists { get { return _personalLists.Values; } }
        public IEnumerable<SeznamList> SharedLists { get { return _sharedLists.Values; } }

        public User(string username, string password)
        {
            Username = username;
            _password = password;
            _personalLists = new Dictionary<string, SeznamList>();
            CreateNewList("Test");
            _sharedLists = new Dictionary<string, SeznamList>();
        }

        public string Username { get; private set; }

        internal void Authenticate(string password)
        {
            if (!_password.Equals(password))
                throw new AuthenticationException();
        }

        public SeznamList CreateNewList(string name)
        {
            if (_personalLists.ContainsKey(name))
                throw new ValidationException("Duplicate list name.");

            var list = new SeznamList { Name = name };
            _personalLists.Add(name, list);
            return list;
        }

        public SeznamList GetPersonalList(string name)
        {
            return _personalLists[name];
        }
    }
}