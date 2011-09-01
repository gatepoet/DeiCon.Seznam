using System.Security.Authentication;
using NUnit.Framework;
using Raven.Client;

namespace Seznam.User.Tests
{
    [TestFixture]
    public abstract class SeznamUserserviceTestsBase
    {
        protected IDocumentStore DocumentStore;

        [SetUp]
        public virtual void Setup() { }

        [Test]
        public void ShouldBeAbleToCreateUser()
        {
            var service = CreateServiceWithNoUsers();

            const string password = "passw0rd";
            const string username = "Jan";
            var userId = service.CreateUser(username, password);


            using (var session = DocumentStore.OpenSession())
            {
                var user = session.Load<Account>(userId);
                Assert.That(user, Is.Not.Null); 
                Assert.That(user.Id, Is.EqualTo(userId)); 
                Assert.That(user.Username, Is.EqualTo(username)); 
                Assert.That(user.Password, Is.EqualTo(password)); 
            }
        }

        [Test]
        public void GivenUserExists_WhenAuthenticatingWithCorrectPassword_ShouldNotThrow()
        {
            const string password = "passw0rd";
            const string username = "Jan";
            var service = CreateServiceWithUser(username, password);

            service.Authenticate(username, password);
        }

        [Test]
        public void GivenUserExists_WhenAuthenticatingWithIncorrectPassword_ShouldThrow()
        {
            const string password = "passw0rd";
            const string username = "Jan";
            var service = CreateServiceWithUser(username, password);

            Assert.Throws<AuthenticationException>(() => service.Authenticate(username, string.Empty));
        }

        [Test]
        public void GivenUserDontExist_WhenAuthenticating_ShouldThrow()
        {
            const string password = "passw0rd";
            const string username = "Jan";
            var service = CreateServiceWithNoUsers();

            Assert.Throws<AuthenticationException>(() => service.Authenticate(username, string.Empty));
        }

        protected abstract UserService CreateServiceWithUser(string username, string password);

        [TearDown]
        public virtual void Teardown() { }

        protected abstract UserService CreateServiceWithNoUsers();
    }
}