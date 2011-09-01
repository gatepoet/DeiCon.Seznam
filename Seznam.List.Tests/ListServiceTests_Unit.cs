using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Seznam.List.Contracts;

namespace Seznam.List.Test
{
    [TestFixture]
    public class ListServiceTests_Unit : ListServiceTestsBase
    {

        public override void Teardown()
        {
            DocumentStore.Dispose();
            DocumentStore = null;
        }

        protected override IListService CreateService()
        {
            var store = new EmbeddableDocumentStore { RunInMemory = true };
            store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
            store.Initialize();
            var service = new ListService(new ListRepository(store));
            DocumentStore = store;

            return service;
        }
    }
}