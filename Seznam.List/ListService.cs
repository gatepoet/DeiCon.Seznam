using System;
using Raven.Client.Document;
using Seznam.List.Contracts;

namespace Seznam.List
{
    public class ListService : IListService
    {
        private readonly ListRepository _repository;

        public ListService(ListRepository repository)
        {
            _repository = repository;
        }

        public ListService()
        {
            var config = Config.Current;
            var url = BuildUrl(config.Host, config.Port);
            var documentStore = new DocumentStore
                                    {
                                        Url = url,
                                        DefaultDatabase = "Seznam.Lists",
                                        Conventions = {DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites}
                                    };
            documentStore.Initialize();
            _repository = new ListRepository(documentStore);
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


        public void Dispose()
        {
            _repository.Dispose();
        }

        public SeznamSummmary GetSummary(string userId)
        {
            var personal = _repository.GetAllByCriteria<SeznamList>(l => l.UserId == userId);
            var shared = _repository.GetAllByCriteria<SeznamList>(l => l.Shared && l.Users.Contains(userId));

            return new SeznamSummmary
                       {
                           PersonalLists = personal,
                           SharedLists = shared
                       };
        }

        public SeznamList CreateList(SeznamList list)
        {
            var created = _repository.StoreSafe(list, l => l.UserId == list.UserId && l.Name == list.Name);

            return created;
        }

        public ItemChangedData CreateListItem(string listId, string name, int count)
        {
            return _repository.CreateNewListItem(listId, name, count);
        }

        public ItemChangedData TogglePersonalItem(string listId, string name, bool completed)
        {
            return _repository.ToggleItem(listId, name, completed);
        }

        public ItemChangedData ToggleSharedItem(string listId, string name, bool completed)
        {
            return _repository.ToggleItem(listId, name, completed);
        }

        public ItemChangedData DeleteItem(string listId, string name)
        {
            return _repository.DeleteItem(listId, name);
        }
    }
}