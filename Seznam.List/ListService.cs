using System;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;
using Raven.Client.Document;
using Seznam.List.Contracts;

namespace Seznam.List
{
    public class ListService : IListService
    {
        private ListRepository _repository;
        private ListRepository Repository { get { return _repository ?? (_repository = CreateRepository()); } }

        private static string GetRandomEnpoint()
        {
            //var endpoints = RoleEnvironment.Roles["Raven"].Instances.Select(i => i.InstanceEndpoints["Raven"]).ToArray();

            //var r = new Random();

            //var endpoint = endpoints[r.Next(endpoints.Count() - 1)].IPEndpoint;
            //var url = String.Format("tcp://{0}:{1}/", endpoint.Address, 8081);
            //var url = BuildUrl(endpoint.Address.ToString(), endpoint.Port);
            var host = HttpContext.Current.Request.Headers["Host"].Split(':')[0];
            var url = BuildUrl(host, Config.Current.Port); 
            return url;
        }

        private ListRepository CreateRepository()
        {
            var documentStore = new DocumentStore
            {
                Url = GetRandomEnpoint(),
                DefaultDatabase = "Seznam.Lists",
            };
            documentStore.Initialize();
            return new ListRepository(documentStore);
        }

        public ListService(ListRepository repository)
        {
            _repository = repository;
        }

        public ListService()
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


        public void Dispose()
        {
            if (_repository != null)
                _repository.Dispose();
        }

        public SeznamSummmary GetSummary(string userId)
        {
            var personal = Repository.GetAllByCriteria<SeznamList>(l => l.UserId == userId);
            var shared = Repository.GetAllByCriteria<SeznamList>(l => l.Shared && l.Users.Contains(userId));

            return new SeznamSummmary
                       {
                           PersonalLists = personal,
                           SharedLists = shared
                       };
        }

        public SeznamList CreateList(SeznamList list)
        {
            var created = Repository.StoreSafe(list, l => l.UserId == list.UserId && l.Name == list.Name);

            return created;
        }

        public ItemChangedData CreateListItem(string listId, string name, int count)
        {
            return Repository.CreateNewListItem(listId, name, count);
        }

        public ItemChangedData TogglePersonalItem(string listId, string name, bool completed)
        {
            return Repository.ToggleItem(listId, name, completed);
        }

        public ItemChangedData ToggleSharedItem(string listId, string name, bool completed)
        {
            return Repository.ToggleItem(listId, name, completed);
        }

        public ItemChangedData DeleteItem(string listId, string name)
        {
            return Repository.DeleteItem(listId, name);
        }
    }
}