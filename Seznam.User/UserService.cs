using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Document;
using Seznam.User.Contracts;

namespace Seznam.User
{
    public class UserService : IUserService
    {
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public UserService()
        {
            var config = Config.Current;
            var url = BuildUrl(config.Host, config.Port);
            var documentStore = new DocumentStore
                                    {
                                        Url = url,
                                        DefaultDatabase = "Seznam.Users",
                                        Conventions = {DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites}
                                    };
            documentStore.Initialize();
            _repository = new UserRepository(documentStore);
        }

        private static string BuildUrl(string host, int port)
        {
            var builder = new UriBuilder
                              {
                                  Host = host,
                                  Port = port
                              };
            return builder.Uri.AbsoluteUri;
        }

        public string CreateUser(string username, string password)
        {
            var account = new Account {Username = username, Password = password};
            var id = _repository.CreateAccount(account);
            return id;
        }

        public string Authenticate(string username, string password)
        {
            return _repository.Authenticate(username, password);
        }

        public IEnumerable<string> GetUserIds(IEnumerable<string> usernames)
        {
            var users = _repository.GetAllByCriteria<Account>(u => usernames.Any(n => n == u.Username));
            return users.Select(u => u.Id);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}