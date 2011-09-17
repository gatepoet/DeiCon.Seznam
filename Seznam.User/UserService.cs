using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Raven.Client.Document;
using Seznam.User.Contracts;

namespace Seznam.User
{


    public class UserService : IUserService
    {
        private UserRepository _repository;
        private UserRepository Repository { get { return _repository ?? (_repository = CreateRepository()); } }

        private UserRepository CreateRepository()
        {
            var config = Config.Current;
            var url = BuildUrl(config.Host, config.Port);
            var documentStore = new DocumentStore
            {
                Url = url,
                DefaultDatabase = "Seznam.Users",
                Conventions = { DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites }
            };
            documentStore.Initialize();
            return new UserRepository(documentStore);
        }

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }


        public UserService()
        {
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
            var id = Repository.CreateAccount(account);
            return id;
        }

        public string Authenticate(string username, string password)
        {
            return Repository.Authenticate(username, password);
        }

        public IEnumerable<string> GetUserIds(IEnumerable<string> usernames)
        {
            var users = Repository.GetAllByCriteria<Account>(u => usernames.Any(n => n == u.Username));
            return users.Select(u => u.Id);
        }

        public void Dispose()
        {
            if (_repository != null)
                _repository.Dispose();
        }
    }
}