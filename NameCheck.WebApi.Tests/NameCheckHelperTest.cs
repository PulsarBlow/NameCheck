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
        public void Should_Return_Sanitized_Value()
        {
            Assert.AreEqual(null, NameCheckHelper.Sanitize(null));
            Assert.AreEqual("", NameCheckHelper.Sanitize(""));

            Assert.AreEqual("A nâme with accent", NameCheckHelper.Sanitize("A nâme with accent"));
            Assert.AreEqual("A Nâme With Accent", NameCheckHelper.Sanitize("A Nâme With Accent"));
            Assert.AreEqual("CamelCased", NameCheckHelper.Sanitize("CamelCased"));
            Assert.AreEqual("camelCased", NameCheckHelper.Sanitize("camelCased"));
            Assert.AreEqual("yesWeCan", NameCheckHelper.Sanitize("yesWeCan"));
            Assert.AreEqual("I Love space ", NameCheckHelper.Sanitize("I Love    space    "));
            Assert.AreEqual("I Love Space ", NameCheckHelper.Sanitize("I Love    Space    "));
            Assert.AreEqual("I Love Spaceéèçàµ", NameCheckHelper.Sanitize("I Love\n\r    Space..!/;,?<>~#{}[]()|`\\^@&é\"'-è_-çà=^¨$%*µ"));
        }

        [TestMethod]
        public void Should_Return_Value_As_FormattedName()
        {
            Assert.AreEqual(null, NameCheckHelper.FormatName(null));
            Assert.AreEqual("", NameCheckHelper.FormatName(""));

            Assert.AreEqual("A nâme with accent", NameCheckHelper.FormatName(" A nâme with accent "));
            Assert.AreEqual("A Nâme With Accent", NameCheckHelper.FormatName("A Nâme With Accent "));
            Assert.AreEqual("CamelCased", NameCheckHelper.FormatName("CamelCased"));
            Assert.AreEqual("camelCased", NameCheckHelper.FormatName("camelCased"));
            Assert.AreEqual("yesWeCan", NameCheckHelper.FormatName("yesWeCan"));
            Assert.AreEqual("I Love space", NameCheckHelper.FormatName("I Love    space    "));
            Assert.AreEqual("I Love Space", NameCheckHelper.FormatName("I Love    Space    "));
            Assert.AreEqual("I Love Spaceéèçàµ", NameCheckHelper.FormatName("I Love\n\r    Space..!/;,?<>~#{}[]()|`\\^@&é\"'-è_-çà=^¨$%*µ"));
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

        [TestMethod]
        public void Should_Return_Value_As_FormattedKey()
        {
            Assert.AreEqual("anamewithaccent", NameCheckHelper.FormatKey(" A nâme with accent"));
            Assert.AreEqual("a-name-with-accent", NameCheckHelper.FormatKey("A Nâme With Accent "));
            Assert.AreEqual("camel-cased", NameCheckHelper.FormatKey("CamelCased"));
            Assert.AreEqual("camel-cased", NameCheckHelper.FormatKey("camelCased"));
            Assert.AreEqual("yes-we-can", NameCheckHelper.FormatKey("yesWeCan"));
            Assert.AreEqual("i-lovespace", NameCheckHelper.FormatKey("I Love    space    "));
            Assert.AreEqual("i-love-space", NameCheckHelper.FormatKey("I Love    Space    "));
            Assert.AreEqual("i-love-spaceeecaµ", NameCheckHelper.FormatKey("I Love\n\r    Space..!/;,?<>~#{[|`\\^@]}&é\"'(-è_-çà)=^¨$%*µ"));
        }
    }
}
