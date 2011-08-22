using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Seznam.Data.Services.List.Contracts;


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

    public class ListRepository : SeznamRepository
    {
        public ListRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public SeznamListItem CreateNewListItem(string listId, string name, int count)
        {
            using (var session = _documentStore.OpenSession())
            {
                var list = session.Load<SeznamList>(listId);
                if (list == null)
                    throw new ListNotFoundException(string.Format("List '{0}' not found when creating item '{1}", listId, name));

                var item = list.AddItem(name, count);
                session.SaveChanges();
                return item;
            }
        }

        public SeznamListItem TogglePersonalListItem(string listId, string name, bool completed)
        {
            using (var session = _documentStore.OpenSession())
            {
                var list = session.Load<SeznamList>(listId);
                if (list == null)
                    throw new ListNotFoundException(string.Format("List '{0}' not found when toggling item '{1}", listId, name));

                var item = list.Items.Single(i => i.Name == name);
                item.Completed = completed;
                session.SaveChanges();

                return item;
            }
        }

        public void DeleteItem(string listId, string name)
        {
            using (var session = _documentStore.OpenSession())
            {
                var list = session.Load<SeznamList>(listId);
                if (list == null)
                    throw new ListNotFoundException(string.Format("List '{0}' not found when deleting item '{1}", listId, name));

                var item = list.Items.Single(i => i.Name == name);
                list.Items.Remove(item);
                session.SaveChanges();
            }
        }
    }
}
