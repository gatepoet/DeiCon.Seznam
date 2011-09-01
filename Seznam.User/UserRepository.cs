using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using Raven.Client;
using Seznam.User.Contracts;

namespace Seznam.User
{
    public class UserRepository : IDisposable
    {
        private readonly IDocumentStore _documentStore;

        public UserRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public string CreateAccount(Account account)
        {
            using (var documentSession = _documentStore.OpenSession())
            {
                var existing = documentSession
                    .Query<Account>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfLastWrite())
                    .Any(a => a.Username == account.Username);

                if (existing)
                    throw new UserExistsException(string.Format("User '{0}' aldready exists.", account.Username));
                
                documentSession.Store(account);
                documentSession.SaveChanges();

                return account.Id;
            }
        }

        public T GetById<T>(string id)
        {
            using (var documentSession = _documentStore.OpenSession())
            {
                var data = documentSession.Load<T>(id);
                return data;
            }
        }
        public T GetByCriteria<T>(Func<T,bool> predicate)
        {
            using (var documentSession = _documentStore.OpenSession())
            {
                var data = documentSession
                    .Query<T>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                    .SingleOrDefault(db => predicate(db));
                return data;
            }
        }

        public void Dispose()
        {
            _documentStore.Dispose();
        }

        public List<T> GetAllByCriteria<T>(Func<T, bool> predicate)
        {
            using (var documentSession = _documentStore.OpenSession())
            {
                var data = documentSession
                    .Query<T>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                    .Where(predicate);
                return data.ToList();
            }
        }

        public string Authenticate(string username, string password)
        {
            using (var documentSession = _documentStore.OpenSession())
            {
                var data = documentSession
                    .Query<Account>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfNow());

                var user = data.SingleOrDefault(db => db.Username == username);
                if (user != null && password.Equals(user.Password))
                    return user.Id;
                else
                    throw new AuthenticationException(string.Format("User {0} failed to log in.", username));
            }
        }
    }
}
