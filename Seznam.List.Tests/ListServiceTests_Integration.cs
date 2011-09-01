using System.Reflection;
using System.Transactions;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Seznam.List.Contracts;

namespace Seznam.List.Test
{
    [TestFixture]
    public class ListServiceTests_Integration : ListServiceTestsBase
    {
        private TransactionScope _tx;

        [SetUp]
        public override void Setup()
        {
            _tx = new TransactionScope();
        }

        [TearDown]
        public override void Teardown()
        {
            ////_tx.Complete();
            _tx.Dispose();
            _tx = null;
            DocumentStore.Dispose();
            DocumentStore = null;
        }

        protected override IListService CreateService()
        {
            Config.SetCurrent(new TestConfig { Host = "localhost", Port = 8081 });
            var service = new ListService();
            DocumentStore = GetDocumentstore(service);
            return service;
        }

        private IDocumentStore GetDocumentstore(ListService service)
        {
            var rep = GetValue(service, "_repository");
            var store = GetValue<DocumentStore>(rep, "_documentStore");
            return store;
        }

        private static T GetValue<T>(object obj, string fieldName) where T : class
        {
            var value = GetValue(obj, fieldName);
            return value as T;
        }
        private static object GetValue(object obj, string fieldName)
        {
            return obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
        }

    }
}