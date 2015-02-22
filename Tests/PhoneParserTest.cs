using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Nepy.Parsers;
using Nepy.Core;
using Nepy.Core;

namespace Nepy.Tests
{
    [TestFixture]
    public class PhoneParserTest
    {
        void AssertPhoneValue(ParseResult pr, string countrycode, string citycode, string main, string extension, bool isMobile)
        {
            Assert.AreEqual(pr.Value.GetType(), typeof(PhoneNo));
            PhoneNo value = (PhoneNo)pr.Value;

            Assert.AreEqual(countrycode, value.CountryCode);
            Assert.AreEqual(citycode, value.AreaCode);
            Assert.AreEqual(main, value.Main);
            Assert.AreEqual(extension, value.Extension);
            Assert.AreEqual(isMobile, value.IsMobile);
        }
        void AssertPhoneValue(ParseResult pr, string countrycode, string citycode, string main, string extension)
        {
            AssertPhoneValue(pr, countrycode, citycode, main, extension, false);
        }

        [Test]
        public void TestParseSinglePhoneWithExtension()
        {
            PhoneNoParser parser = new PhoneNoParser(TestUtility.CreateParserContext("+86 21 62253302#302", ParserPattern.China));
            ParseResultCollection prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "+86 21 62253302#302", 0, POSType.A_M);
            AssertPhoneValue(prc[0], "86", "21", "62253302", "302");

            parser = new PhoneNoParser(TestUtility.CreateParserContext("+86 21 62253302#302", ParserPattern.China));
            prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "+86 21 62253302#302", 0, POSType.A_M);
            AssertPhoneValue(prc[0], "86", "21", "62253302", "302");

            parser = new PhoneNoParser(TestUtility.CreateParserContext("+86 21 62253302#302a", ParserPattern.China));
            prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "+86 21 62253302#302", 0, POSType.A_M);
            AssertPhoneValue(prc[0], "86", "21", "62253302", "302");
        }
        [Test]
        public void TestParseSingleNorthAmericanPhone()
        {
            PhoneNoParser parser = new PhoneNoParser(TestUtility.CreateParserContext("+1-415-555-2374", ParserPattern.NorthAmerica));
            ParseResultCollection prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "+1-415-555-2374", 0, POSType.A_M);
            AssertPhoneValue(prc[0], "1", "415", "555-2374", null, true);

            parser = new PhoneNoParser(TestUtility.CreateParserContext("(800) 628-1058", ParserPattern.NorthAmerica));
            prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "(800) 628-1058", 0, POSType.A_M);
            AssertPhoneValue(prc[0], null, "800", "628-1058", null, true);
        }
        [Test]
        public void TestParseSingleMobile()
        {
            PhoneNoParser parser2 = new PhoneNoParser(TestUtility.CreateParserContext("+86 13482572088", ParserPattern.China));
            ParseResultCollection prc2 = parser2.Parse(0);
            Assert.AreEqual(1, prc2.Count);
            TestUtility.AssertParseResult(prc2[0], "+86 13482572088", 0, POSType.A_M);
            AssertPhoneValue(prc2[0], "86", null, "13482572088", null, true);
        }
        [Test]
        public void TestParseSingleIllegalPhone()
        {
            PhoneNoParser parser = new PhoneNoParser(TestUtility.CreateParserContext(" ", ParserPattern.China));
            ParseResultCollection prc = parser.Parse(0);
            Assert.AreEqual(0, prc.Count);

            parser = new PhoneNoParser(TestUtility.CreateParserContext("611212341", ParserPattern.China));
            prc = parser.Parse(0);
            Assert.AreEqual(0, prc.Count);

            parser = new PhoneNoParser(TestUtility.CreateParserContext("2", ParserPattern.China));
            prc = parser.Parse(0);
            Assert.AreEqual(0, prc.Count);

            parser = new PhoneNoParser(TestUtility.CreateParserContext("23", ParserPattern.China));
            prc = parser.Parse(0);
            Assert.AreEqual(0, prc.Count);
        }
        [Test]
        public void TestParseSinglePhone()
        {
            PhoneNoParser parser = new PhoneNoParser(TestUtility.CreateParserContext("（０２１）６６５５　２４３３", ParserPattern.China));
            ParseResultCollection prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "(021)6655 2433", 0, POSType.A_M);
            AssertPhoneValue(prc[0], null, "021", "6655 2433", null);

            parser = new PhoneNoParser(TestUtility.CreateParserContext("021-64393615", ParserPattern.China));
            prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "021-64393615", 0, POSType.A_M);
            AssertPhoneValue(prc[0], null, "021", "64393615", null);

            PhoneNoParser parser2 = new PhoneNoParser(TestUtility.CreateParserContext("+86 21 6225　3302", ParserPattern.China));
            ParseResultCollection prc2 = parser2.Parse(0);
            Assert.AreEqual(1, prc2.Count);
            TestUtility.AssertParseResult(prc2[0], "+86 21 6225 3302", 0, POSType.A_M);
            AssertPhoneValue(prc2[0], "86", "21", "6225 3302", null);

            parser2 = new PhoneNoParser(TestUtility.CreateParserContext("电话号码：62253302", ParserPattern.China));
            prc2 = parser2.Parse(0);
            Assert.AreEqual(0, prc2.Count);

            prc2 = parser2.Parse(5);
            Assert.AreEqual(1, prc2.Count);
            TestUtility.AssertParseResult(prc2[0], "62253302", 5, POSType.A_M);
            AssertPhoneValue(prc2[0], null, null, "62253302", null);

            parser2 = new PhoneNoParser(TestUtility.CreateParserContext("13566223388(手机)", ParserPattern.China));
            prc2 = parser2.Parse(0);
            Assert.AreEqual(1, prc2.Count);
            TestUtility.AssertParseResult(prc2[0], "13566223388", 0, POSType.A_M);
        }
        [Test]
        public void TestIsMobileNoForChina()
        {
            Assert.IsTrue(PhoneNoParser.IsMobileNo("13035564883", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("13482572088", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("13564542929", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("13681655868", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("13788998731", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("13810000018", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("13910000086", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("13901762128", ParserPattern.China));


            Assert.IsTrue(PhoneNoParser.IsMobileNo("18221980993", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("18717717763", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("18801858876", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("18901900000", ParserPattern.China));


            Assert.IsTrue(PhoneNoParser.IsMobileNo("15021133318", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("15221071113", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("15821503088", ParserPattern.China));
            Assert.IsTrue(PhoneNoParser.IsMobileNo("15901723239", ParserPattern.China));


            Assert.IsFalse(PhoneNoParser.IsMobileNo("1303556488", ParserPattern.China));     //位数不够
            Assert.IsFalse(PhoneNoParser.IsMobileNo("15411223344", ParserPattern.China));    //154开头
            Assert.IsFalse(PhoneNoParser.IsMobileNo("25411223344", ParserPattern.China));    //首位2开头
            Assert.IsFalse(PhoneNoParser.IsMobileNo("16411223344", ParserPattern.China));    //16开头
            Assert.IsFalse(PhoneNoParser.IsMobileNo("18111223344", ParserPattern.China));    //181开头

        }
        ParseResultCollection ParsePhoneNo(string text)
        {
            ParserContext context = new ParserContext();
            context.Text = text;
            return ParseResultCollection.InternalParse(text, new PhoneNoParser(context));
        }
        [Test]
        public void TestParsePhones()
        {
            string text = "机关党工委  86780445 区政研室  86780455 老干部局  86780474  团区委  86780515 妇  联  86780524";
            ParseResultCollection prc = ParsePhoneNo(text);
            Assert.AreEqual(5, prc.Count);
            TestUtility.AssertParseResult(prc[0], "86780445", 7, POSType.A_M);
            AssertPhoneValue(prc[0], null, null, "86780445", null);
            TestUtility.AssertParseResult(prc[1], "86780455", 22, POSType.A_M);
            TestUtility.AssertParseResult(prc[2], "86780474", 37, POSType.A_M);
            TestUtility.AssertParseResult(prc[3], "86780515", 52, POSType.A_M);
            TestUtility.AssertParseResult(prc[4], "86780524", 67, POSType.A_M);
        }
    }
}
