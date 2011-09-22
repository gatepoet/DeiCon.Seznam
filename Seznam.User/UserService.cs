using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        private static string GetRandomEnpoint()
        {
            //var endpoints = RoleEnvironment.Roles["Raven"].Instances.Select(i => i.InstanceEndpoints["Raven"]).ToArray();

            //var r = new Random();

            //var endpoint = RoleEnvironment.Roles["Raven"].Instances.First().InstanceEndpoints.First().Value.IPEndpoint;
            //var url = BuildUrl(endpoint.Address.ToString(), Config.Current.Port);
            var host = HttpContext.Current.Request.Headers["Host"].Split(':')[0];
            var url = BuildUrl(host, Config.Current.Port);
            return url;
        }

        private UserRepository CreateRepository()
        {
            var documentStore = new DocumentStore
            {
                Url = GetRandomEnpoint(),
                DefaultDatabase = "Seznam.Users",
                //Conventions = { DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites }
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