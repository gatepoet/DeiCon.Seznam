using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using Raven.Client;
using Seznam.Data.Services.List.Contracts;
using Seznam.Data.Services.User;


namespace Seznam.Data
{
    public class SeznamRepository : IRepository, IDisposable
    {
        protected readonly IDocumentStore _documentStore;

        public SeznamRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public T Store<T>(T data)
        {
            using (var documentSession = _documentStore.OpenSession())
            {
                documentSession.Store(data);
                documentSession.SaveChanges();
            }
            return data;
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

        public T StoreSafe<T>(T data, Func<T, bool> predicate) where T : class
        {
            using (var documentSession = _documentStore.OpenSession())
            {
                var existing = documentSession
                    .Query<T>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                    .Any(predicate);

                if (existing)
                    throw new DataExistsException("Data already exists");

                documentSession.Store(data);
                documentSession.SaveChanges();

                return data;
            }
        }
    }

    public class UserRepository : SeznamRepository
    {
        public UserRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public string Authenticate(string username, string password)
        {
            using (var documentSession = _documentStore.OpenSession())
            {
                var data = documentSession
                    .Query<User>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfNow());

                var user = data.SingleOrDefault(db => db.Username == username);
                if (user != null && password.Equals(user.Password))
                    return user.Id;
                else
                    throw new AuthenticationException(string.Format("User {0} failed to log in.", username));
            }
        }

    }

    public class ListRepository : SeznamRepository
    {
        public ListRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public ItemChangedData CreateNewListItem(string listId, string name, int count)
        {
            using (var session = _documentStore.OpenSession())
            {
                var list = session.Load<SeznamList>(listId);
                if (list == null)
                    throw new ListNotFoundException(string.Format("List '{0}' not found when creating item '{1}", listId, name));

                var item = list.AddItem(name, count);
                session.SaveChanges();
                return new ItemChangedData { List = list, Item = item };
            }
        }

        public ItemChangedData ToggleItem(string listId, string name, bool completed)
        {
            using (var session = _documentStore.OpenSession())
            {
                var list = session.Load<SeznamList>(listId);
                if (list == null)
                    throw new ListNotFoundException(string.Format("List '{0}' not found when toggling item '{1}", listId, name));

                var item = list.Items.Single(i => i.Name == name);
                item.Completed = completed;
                session.SaveChanges();

                return new ItemChangedData { List = list, Item = item };
            }
        }

        public ItemChangedData DeleteItem(string listId, string name)
        {
            using (var session = _documentStore.OpenSession())
            {
                var list = session.Load<SeznamList>(listId);
                if (list == null)
                    throw new ListNotFoundException(string.Format("List '{0}' not found when deleting item '{1}", listId, name));

                var item = list.Items.Single(i => i.Name == name);
                list.Items.Remove(item);
                session.SaveChanges();

                return new ItemChangedData { List = list, Item = item };
            }
        }
    }

    public class ItemChangedData
    {
        public SeznamList List { get; set; }
        public SeznamListItem Item { get; set; }
    }
}
