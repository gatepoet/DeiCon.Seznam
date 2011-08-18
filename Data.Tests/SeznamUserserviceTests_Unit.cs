using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Seznam.Data.Services.User;

namespace Seznam.Data.Tests
{
    public class SeznamUserserviceTests_Unit : SeznamUserserviceTestsBase
    {
        public override void Teardown()
        {
            DocumentStore.Dispose();
            DocumentStore = null;
        }

        protected override UserService CreateServiceWithNoUsers()
        {
            DocumentStore = new EmbeddableDocumentStore { RunInMemory = true };
            DocumentStore.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
            DocumentStore.Initialize();
            var service = new UserService(new SeznamRepository(DocumentStore));
            return service;
        }

        protected override UserService CreateServiceWithUser(string username, string password)
        {
            DocumentStore = new EmbeddableDocumentStore { RunInMemory = true };
            DocumentStore.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;

            DocumentStore.Initialize();
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(new User{Username = username, Password = password});
                session.SaveChanges();
            }
            var service = new UserService(new SeznamRepository(DocumentStore));

            return service;
        }
    }
}