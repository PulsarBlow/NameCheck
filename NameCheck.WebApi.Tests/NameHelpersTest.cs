using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NameCheck.WebApi.Tests
{
    [TestClass]
    public class NameHelpersTest
    {
        [TestMethod]
        public void Should_Return_Name_Without_Extension()
        {
            Assert.AreEqual("aname", NameHelpers.RemoveExtension("aname.com"));
            Assert.AreEqual("aname", NameHelpers.RemoveExtension("aname"));
            Assert.AreEqual("", NameHelpers.RemoveExtension(""));
            Assert.AreEqual(null, NameHelpers.RemoveExtension(null));
            Assert.AreEqual("aname", NameHelpers.RemoveExtension("aname.net"));
            Assert.AreEqual("aname", NameHelpers.RemoveExtension("aname.io"));
        }
    }
}
