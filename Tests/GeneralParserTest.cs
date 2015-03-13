using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Nepy.Core;
using Nepy.Parsers;

namespace BluePrint.SegmentFramework.UnitTest
{
    [TestFixture]
    public class GeneralParserTest
    {
        public static void AssertParseResult(ParseResult pr, string text, int startPos, POSType type)
        {
            Assert.AreEqual(text, pr.Text);
            Assert.AreEqual(startPos, pr.StartPos);
            Assert.AreEqual(type, pr.Type);
        }
        public static ParserContext CreateParserContext(string text, ParserPattern pattern)
        {
            ParserContext pc = new ParserContext();
            pc.Pattern = pattern;
            pc.Text = text;
            return pc;
        }
        //public static ParserContext CreateParserContext(string text, string serverAddr)
        //{
        //    ParserContext context = new ParserContext(serverAddr);
        //    var client = context.GetServerInstance();
        //    context.Text = text;
        //    return context;
        //}
        public static ParserContext CreateParserContext(string text)
        {
            return CreateParserContext(text, ParserPattern.China);
        }

        [Test]
        public void TestParseSinglePostalCode()
        {
            IParser p = new PostalCodeParser(GeneralParserTest.CreateParserContext("200135，哈哈",ParserPattern.China));
            ParseResultCollection prc = p.Parse(0);
            Assert.AreEqual(1, prc.Count);
            AssertParseResult(prc[0], "200135", 0, POSType.A_M);

            p = new PostalCodeParser(GeneralParserTest.CreateParserContext("21201　　Baltimore          Maryland(MD)", ParserPattern.NorthAmerica));
            prc = p.Parse(0);
            Assert.AreEqual(1, prc.Count);
            AssertParseResult(prc[0], "21201", 0, POSType.A_M);
        }
    }
}
