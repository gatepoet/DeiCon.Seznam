using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Raven.Client.Document;
using Seznam.Data.Services.List.Contracts;

namespace Seznam.Data.Services.List
{
    public class ListService : IListService, IDisposable
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
            var shared = _repository.GetAllByCriteria<SeznamList>(l => l.Users.Contains(userId));

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

        public SeznamListItem CreateListItem(string listId, string name, int count)
        {
            return _repository.CreateNewListItem(listId, name, count);
        }

        public SeznamListItem TogglePersonalListItem(string listId, string name, bool completed)
        {
            return _repository.TogglePersonalListItem(listId, name, completed);
        }

        public void DeleteItem(string listId, string name)
        {
            _repository.DeleteItem(listId, name);
        }
    }
}