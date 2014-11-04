using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace NameCheck.WebApi.Tests
{
    [TestClass]
    public class AuthorizedUserStoreTest
    {
        [TestMethod]
        public void CreateStoreFromConfigTest()
        {
            // Check app.config for test configuration
            PrivateType type = new PrivateType(typeof(AuthorizedUserStore));
            Dictionary<string, List<ApplicationUser>> store = type.InvokeStatic("CreateStoreFromConfig", null) as Dictionary<string, List<ApplicationUser>>;

            Assert.IsNotNull(store);
            Assert.IsTrue(store.Count == 2);
            Assert.IsTrue(store.ContainsKey("domain.com"));
            Assert.IsTrue(store.ContainsKey("domain_alt.com"));

            var domainUsers = store["domain.com"];
            var domainAltUsers = store["domain_alt.com"];

            Assert.IsTrue(domainUsers.Count == 2);
            Assert.AreEqual("domain.com", domainUsers[0].Domain);
            Assert.AreEqual("*", domainUsers[0].UserName);
            Assert.AreEqual("*@domain.com", domainUsers[0].Email);
            Assert.AreEqual("domain.com", domainUsers[1].Domain);
            Assert.AreEqual("jeff", domainUsers[1].UserName);
            Assert.AreEqual("jeff@domain.com", domainUsers[1].Email);

            Assert.IsTrue(domainAltUsers.Count == 2);
            Assert.AreEqual("domain_alt.com", domainAltUsers[0].Domain);
            Assert.AreEqual("tina", domainAltUsers[0].UserName);
            Assert.AreEqual("tina@domain_alt.com", domainAltUsers[0].Email);
            Assert.AreEqual("domain_alt.com", domainAltUsers[1].Domain);
            Assert.AreEqual("joe", domainAltUsers[1].UserName);
            Assert.AreEqual("joe@domain_alt.com", domainAltUsers[1].Email);
        }

        [TestMethod]
        public void IsAuthorizedTest()
        {
            // Check app.config for test configuration
            AuthorizedUserStore store = new AuthorizedUserStore();
            Assert.IsTrue(store.IsAuthorized("jeff@domain.com"));
            Assert.IsTrue(store.IsAuthorized(" jeff@domain.com"));
            Assert.IsTrue(store.IsAuthorized("jeff@domain.com "));
            Assert.IsTrue(store.IsAuthorized(" jeff@domain.com "));
            Assert.IsTrue(store.IsAuthorized("dude@domain.com")); // * has been configurated
            Assert.IsTrue(store.IsAuthorized("tina@domain_alt.com"));
            Assert.IsTrue(store.IsAuthorized(" tina@domain_alt.com"));
            Assert.IsTrue(store.IsAuthorized("tina@domain_alt.com "));
            Assert.IsTrue(store.IsAuthorized(" tina@domain_alt.com "));
            Assert.IsTrue(store.IsAuthorized("joe@domain_alt.com"));


            Assert.IsFalse(store.IsAuthorized("@domain.com"));
            Assert.IsFalse(store.IsAuthorized("jeff@domain.net"));
            Assert.IsFalse(store.IsAuthorized("georges@domain_alt.com"));
            Assert.IsFalse(store.IsAuthorized("tina@domainalt.com"));
            Assert.IsFalse(store.IsAuthorized("tinâ@domainalt.com"));
        }
    }
}
