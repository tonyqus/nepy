using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Nepy.Parsers;
using Nepy.Core;

namespace Nepy.Tests
{
    [TestFixture]
    public class SimChinesePronounParserTest
    {
        ParserContext CreateParserContext(string text)
        {
            return TestUtility.CreateParserContext(text);
        }

        [Test]
        public void TestnParsePersonalNou()
        {
            IParser parser = new SimChinesePronounParser(CreateParserContext("你们是我师兄。"));
            ParseResultCollection prc=parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "你们", 0, POSType.D_R);
            prc = parser.Parse(3);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "我", 3, POSType.D_R);

            parser.Context.Text = "快看，它们是可爱的小狗狗";
            prc = parser.Parse(3);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "它们", 3, POSType.D_R);

            parser.Context.Text = "是朕，还不快来救驾";
            prc = parser.Parse(1);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "朕", 1, POSType.D_R);
        }

        [Test]
        public void TestParseDemostrativeNoun()
        {
            IParser parser = new SimChinesePronounParser(CreateParserContext("《那些年我们一起追过的女孩》"));
            ParseResultCollection prc = parser.Parse(1);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "那些", 1, POSType.D_R);
            prc = parser.Parse(4);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "我们", 4, POSType.D_R);

            parser.Context.Text = "这儿的花别样红";
            prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "这儿", 0, POSType.D_R);

            parser.Context.Text = "欢迎来到这里——FBI";
            prc = parser.Parse(4);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "这里", 4, POSType.D_R);
        }

        [Test]
        public void TestParseQuestionNoun()
        {
            IParser parser = new SimChinesePronounParser(CreateParserContext("这里是谁负责的"));
            ParseResultCollection prc = parser.Parse(3);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "谁", 3, POSType.D_R);

            parser.Context.Text="这到底是什么东西？";
            prc = parser.Parse(4);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "什么", 4, POSType.D_R);

            parser.Context.Text = "在哪里呀，在哪里";
            prc = parser.Parse(1);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "哪里", 1, POSType.D_R);
            prc = parser.Parse(6);
            Assert.AreEqual(1, prc.Count);
            TestUtility.AssertParseResult(prc[0], "哪里", 6, POSType.D_R);
        }
    }
}
