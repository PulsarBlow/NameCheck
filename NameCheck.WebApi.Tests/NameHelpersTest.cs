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
            Assert.AreEqual("aname", NameHelper.RemoveExtension("aname.com"));
            Assert.AreEqual("aname", NameHelper.RemoveExtension("aname"));
            Assert.AreEqual("", NameHelper.RemoveExtension(""));
            Assert.AreEqual(null, NameHelper.RemoveExtension(null));
            Assert.AreEqual("aname", NameHelper.RemoveExtension("aname.net"));
            Assert.AreEqual("aname", NameHelper.RemoveExtension("aname.io"));
        }
    }
}
