using System.Reflection;
using System.Transactions;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Data.Tests;
using Seznam.Data.Services.User;

namespace Seznam.Data.Tests
{
    public class SeznamUserserviceTests_Integration : SeznamUserserviceTestsBase
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

        }

        protected override UserService CreateServiceWithUser(string username, string password)
        {
            Config.SetCurrent(new TestConfig { Host = "localhost", Port = 8081 });
            var service = new UserService();
            DocumentStore = GetDocumentstore(service);
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(new User{Username = username, Password = password});
                session.SaveChanges();
            }
            service.CreateUser(username, password);
            
            return service;
        }

        protected override UserService CreateServiceWithNoUsers()
        {
            Config.SetCurrent(new TestConfig{Host="localhost", Port=8081});
            var service = new UserService();
            DocumentStore = GetDocumentstore(service);
            return service;
        }

        private IDocumentStore GetDocumentstore(UserService service)
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