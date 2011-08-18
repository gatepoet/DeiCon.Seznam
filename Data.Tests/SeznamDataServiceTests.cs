using NUnit.Framework;
using Raven.Client.Embedded;
using Raven.Http;

namespace Seznam.Data.Tests
{
    public class SeznamDataServiceTests
    {


        [Test]
        public void ShouldBeAbleToAddAndGet()
        {
            var documentStore = new EmbeddableDocumentStore { RunInMemory = true };
            documentStore.Initialize();

            var service = new SeznamRepository(documentStore);
            var testData = new TestData { Name = "test", Count = 0 };

            var storedData = service.Store(testData);
            var result = service.GetById<TestData>(storedData.Id);

            Assert.That(result, Is.EqualTo(testData));
        }

        [Test]
        public void ShouldBeAbleToGetByName()
        {
            using (var documentStore = new EmbeddableDocumentStore { RunInMemory = true })
            {
                documentStore.Initialize();

                var service = new SeznamRepository(documentStore);
                var testData = new TestData { Name = "test", Count = 0 };

                var storedData = service.Store(testData);
                var result = service.GetByCriteria<TestData>(d => d.Name == testData.Name);

                Assert.That(result, Is.EqualTo(testData));
                
            }
        }
    }
}