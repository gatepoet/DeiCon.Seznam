using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using Raven.Client.Document;
using Seznam.Data.Services.User.Contracts;

namespace Seznam.Data.Services.User
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
            try
            {
                var user = _repository.Store(new User { Username = username, Password = password });
                return user.Id;

            }
            catch (Exception e)
            {
                throw new UserExistsException(string.Format("User '{0}' aldready exists.", username), e);
            }
        }

        public string Authenticate(string username, string password)
        {
            return _repository.Authenticate(username, password);
            //var user = _repository.GetByCriteria<User>(u => u.Username == username);

            //if (user != null && password.Equals(user.Password))
            //    return user.Id;
            //else
            //    throw new AuthenticationException(string.Format("User {0} failed to log in.",username));
        }

        public IEnumerable<string> GetUserIds(IEnumerable<string> usernames)
        {
            var users = _repository.GetAllByCriteria<User>(u => usernames.Any(n => n == u.Username));
            return users.Select(u => u.Id);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}