using Nepy.Core;
using Nepy.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Tests
{
    [TestFixture]
    public class PostalCodeParserTest
    {
        [Test]
        public void TestParseSinglePostalCode()
        {
            IParser p = new PostalCodeParser(TestUtility.CreateParserContext("200135，哈哈", ParserPattern.China));
            ParseResultCollection prc = p.Parse(0);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "200135", 0, POSType.A_M);

            p = new PostalCodeParser(TestUtility.CreateParserContext("21201　　Baltimore          Maryland(MD)", ParserPattern.NorthAmerica));
            prc = p.Parse(0);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "21201", 0, POSType.A_M);
        }
    }
}
