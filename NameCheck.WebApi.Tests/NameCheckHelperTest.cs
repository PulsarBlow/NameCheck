using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NameCheck.WebApi.Tests
{
    [TestClass]
    public class NameCheckHelperTest
    {
        [TestMethod]
        public void Should_Return_Value_Without_Extension()
        {
            Assert.AreEqual("aname", NameCheckHelper.RemoveExtension("aname.com"));
            Assert.AreEqual("aname", NameCheckHelper.RemoveExtension("aname"));
            Assert.AreEqual("", NameCheckHelper.RemoveExtension(""));
            Assert.AreEqual(null, NameCheckHelper.RemoveExtension(null));
            Assert.AreEqual("aname", NameCheckHelper.RemoveExtension("aname.net"));
            Assert.AreEqual("aname", NameCheckHelper.RemoveExtension("aname.io"));
        }

        [TestMethod]
        public void Should_Return_Value_As_FormattedKey()
        {
            Assert.AreEqual("anamewithaccent", NameCheckHelper.FormatKey("A nâme with accent"));
            Assert.AreEqual("a-name-with-accent", NameCheckHelper.FormatKey("A Nâme With Accent"));
            Assert.AreEqual("camel-cased", NameCheckHelper.FormatKey("CamelCased"));
            Assert.AreEqual("camel-cased", NameCheckHelper.FormatKey("camelCased"));
            Assert.AreEqual("yes-we-can", NameCheckHelper.FormatKey("yesWeCan"));
            Assert.AreEqual("i-lovespace", NameCheckHelper.FormatKey("I Love    space    "));
            Assert.AreEqual("i-love-space", NameCheckHelper.FormatKey("I Love    Space    "));
        }

        [TestMethod]
        public void Should_Return_Value_As_FormattedQuery()
        {
            Assert.AreEqual("anâmewithaccent", NameCheckHelper.FormatQuery("A nâme with accent")); // domains can have diacritics http://tools.ietf.org/html/rfc3987
            Assert.AreEqual("camelcased", NameCheckHelper.FormatQuery("CamelCased"));
            Assert.AreEqual("camelcased", NameCheckHelper.FormatQuery("camelCased"));
            Assert.AreEqual("yeswecan", NameCheckHelper.FormatQuery("yesWeCan"));
            Assert.AreEqual("ilovespace", NameCheckHelper.FormatQuery("I Love    space    "));
            Assert.AreEqual("cupercoop", NameCheckHelper.FormatQuery("CuperCoop.com"));
        }
    }
}
