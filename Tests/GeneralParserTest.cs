using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BluePrint.NLPCore;
using BluePrint.SegmentFramework.Parser;

namespace BluePrint.SegmentFramework.UnitTest
{
    [TestClass]
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
            
            var client = pc.GetServerInstance();
            return pc;
        }
        public static ParserContext CreateParserContext(string text, string serverAddr)
        {
            ParserContext context = new ParserContext(serverAddr);
            var client = context.GetServerInstance();
            context.Text = text;
            return context;
        }
        public static ParserContext CreateParserContext(string text)
        {
            return CreateParserContext(text, ParserPattern.China);
        }

        [TestMethod]
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
        [TestMethod]
        public void TestParseEnglish()
        {
            IParser p = new EnglishParser(GeneralParserTest.CreateParserContext("This is a test."));
            ParseResultCollection prc = p.Parse(0);
            AssertParseResult(prc[0], "This", 0, POSType.A_NX);
            AssertParseResult(prc[1], "is", 5, POSType.A_NX);
            AssertParseResult(prc[2], "a", 8, POSType.A_NX);
            AssertParseResult(prc[3], "test", 10, POSType.A_NX);
            Assert.AreEqual(4, prc.Count);

            //"our $400 blender can't handle something this hard!"
            p = new EnglishParser(GeneralParserTest.CreateParserContext("our new blender can't handle something this hard!", ParserPattern.NorthAmerica));
            prc = p.Parse(0);

            AssertParseResult(prc[0], "our", 0, POSType.A_NX);
            AssertParseResult(prc[1], "new", 4, POSType.A_NX);
            AssertParseResult(prc[2], "blender", 8, POSType.A_NX);
            AssertParseResult(prc[3], "can't", 16, POSType.A_NX);
            AssertParseResult(prc[4], "handle", 22, POSType.A_NX);
            AssertParseResult(prc[5], "something", 29, POSType.A_NX);
            AssertParseResult(prc[6], "this", 39, POSType.A_NX);
            AssertParseResult(prc[7], "hard", 44, POSType.A_NX);
            Assert.AreEqual(8, prc.Count);
        }
    }
}
