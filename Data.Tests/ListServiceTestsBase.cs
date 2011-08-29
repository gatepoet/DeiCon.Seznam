using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Seznam.Data.Services.List;
using Seznam.Data.Services.List.Contracts;

namespace Seznam.Data.Tests
{
    public abstract class ListServiceTestsBase
    {
        protected IDocumentStore DocumentStore;
        protected IListService Service;

        [SetUp]
        public virtual void Setup(){}

        [TestFixtureSetUp]
        public virtual void FixtureSetup(){}

        [TestFixtureTearDown]
        public virtual void FixtureTeardown(){}

        [TearDown]
        public virtual void Teardown(){}


        [Test]
        public void GivenNoRecordsForUserId_WhenGettingSummary_ShouldReturnEmptyLists()
        {
            var service = CreateService();

            var summary = service.GetSummary("", "");

            Assert.That(summary, Is.Not.Null);
            Assert.That(summary.PersonalLists, Is.Not.Null);
            Assert.That(summary.PersonalLists, Is.Empty);
            Assert.That(summary.SharedLists, Is.Not.Null);
            Assert.That(summary.SharedLists, Is.Empty);
        }

        [Test]
        public void GivenStoredList_WhenGettingSummary_ShouldReturnList()
        {
            var service = CreateService();
            SeznamList list = new SeznamList("Id", "Test", false, string.Empty);
            SeznamList result = service.CreateList(list);
            
            var summary = service.GetSummary(list.UserId, "");
            Assert.That(summary, Is.Not.Null);
            Assert.That(summary.PersonalLists, Is.Not.Null);
            Assert.That(summary.PersonalLists, Is.Not.Empty);
            Assert.That(summary.PersonalLists, Contains.Item(result));
        }
        protected abstract IListService CreateService();

        [Test]
        public void GivenTwoStoredLists_WhenGettingSummary_ShouldReturnBothLists()
        {
            var service = CreateService();
            var userId = "Id";
            var list1 = new SeznamList(userId, "Test", false, string.Empty);
            var list2 = new SeznamList(userId, "Test2", true, "John, Meyer, Ciber");
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(list1);
                session.Store(list2);
                session.SaveChanges();
            }
            
            var summary = service.GetSummary(list1.UserId, "");
            Assert.That(summary, Is.Not.Null);
            Assert.That(summary.PersonalLists, 
                Is.Not.Null.
                And.Not.Empty.
                And.Contains(list1).
                And.Contains(list2));
        }

        [Test]
        public void ShouldBeAbleToCreateNewList()
        {
            var service = CreateService();
            SeznamList list = new SeznamList("Id", "Test", false, string.Empty);
            SeznamList result = service.CreateList(list);

            using (var session = DocumentStore.OpenSession())
            {
                var user = session.Query<SeznamList>().Customize(c => c.WaitForNonStaleResultsAsOfNow()).Single();
                //var user = session.Load<SeznamList>(result.UserId);
                Assert.That(user, Is.Not.Null);
                Assert.That(user, Is.EqualTo(result));
            }

        }

        [Test]
        public void GivenListAlreadyExists_WhenCreatingList_ShouldThrow()
        {
            var service = CreateService();
            SeznamList list = new SeznamList("Id", "Test", false, string.Empty);
            service.CreateList(list);

            Assert.Throws<DataExistsException>(() => service.CreateList(list));
        }


        [Test]
        public void GivenNoListPresent_WhenCreatingListItem_ShouldThrow()
        {
            var service = CreateService();

            Assert.Throws<ListNotFoundException>(() => service.CreateListItem("Test", "test", 0));
        }
        [Test]
        public void GivenListPresent_WhenCreatingListItem_ShouldCreateItem()
        {
            var service = CreateService();
            var list = new SeznamList("Id", "Name", true);
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(list);
                session.SaveChanges();
            }

            var item = service.CreateListItem(list.Id, "Test", 0);

            using (var session = DocumentStore.OpenSession())
            {
                var l = session.Load<SeznamList>(list.Id);
                Assert.That(l, Is.Not.Null);
                Assert.That(l.Items, Is.Not.Null.And.Contains(item));
            }
        }
        [Test]
        public void GivenListAndItemWithSameNamePresent_WhenCreatingListItem_ShouldThrow()
        {
            var service = CreateService();
            var list = new SeznamList("Id", "Name", true);
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(list);
                session.SaveChanges();
            }

            service.CreateListItem(list.Id, "Test", 0);

            Assert.Throws<ListItemExistsException>(() => service.CreateListItem(list.Id, "Test", 0));
        }

        [Test]
        public void GivenListAndItemPresent_WhenTogglingItem_ShouldToggleItem()
        {
            var service = CreateService();
            var list = new SeznamList("Id", "Name", true);
            var item = list.AddItem("Name", 0);
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(list);
                session.SaveChanges();
            }

            var toggledItem = service.TogglePersonalListItem(list.Id, item.Name, true);

            Assert.That(toggledItem.Completed, Is.Not.EqualTo(item.Completed));
        }

        [Test]
        public void GivenOneListPresent_WhenCreatingList_ShouldNotThrow()
        {
            var service = CreateService();
            var list1 = new SeznamList("Id", "Name", false);
            var list2 = new SeznamList("Id2", "Name2", false);
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(list1);
                session.SaveChanges();
            }

            var item = service.CreateList(list2);

            using (var session = DocumentStore.OpenSession())
            {
                var l = session.Load<SeznamList>(item.Id);
                Assert.That(l, Is.EqualTo(item));
            }
        }
        [Test]
        public void GivenOneListPresent_WhenCreatingListWithSameName_ShouldThrow()
        {
            var service = CreateService();
            var list = new SeznamList("Id", "Name", false);
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(list);
                session.SaveChanges();
            }

            Assert.Throws<DataExistsException>(() => service.CreateList(list));
        }

        [Test]
        public void GivenListAndItemPresent_WhenDeletingItem_ShouldDeleteItem()
        {
            var service = CreateService();
            var list = new SeznamList("Id", "Name", false);
            var item = list.AddItem("item", 2);
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(list);
                session.SaveChanges();
            }

            service.DeleteItem(list.Id, item.Name);

            using (var session = DocumentStore.OpenSession())
            {
                var l = session.Load<SeznamList>(list.Id);
                Assert.That(l.Items.Any(i => i.Name == item.Name), Is.False);
            }
        }
        [Test]
        public void GivenIsSharedWith_WhenGettingSummary_ShouldReturnSharedList()
        {
            var service = CreateService();
            var list = new SeznamList("User1", "Name", true, "User2");
            list.AddItem("item", 2);
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(list);
                session.SaveChanges();
            }

            var summary = service.GetSummary("", "User2");

            Assert.That(summary, Is.Not.Null);
            Assert.That(summary.SharedLists, Contains.Item(list));
        }

    }

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