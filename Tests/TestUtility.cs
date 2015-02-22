using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Nepy.Core;
using NUnit.Framework;
using Nepy.Parsers;

namespace Nepy.Tests
{
    public class TestUtility
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
        public static ParserContext CreateParserContext(string text)
        {
            return CreateParserContext(text, ParserPattern.China);
        }
    }
}
