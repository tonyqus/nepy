using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BluePrint.SegmentFramework.Parser;
using BluePrint.NLPCore;

namespace BluePrint.SegmentFramework.UnitTest
{
    [TestClass]
    public class PlaceNameParserTest
    {

        const string dictAddr = TestConfig.dictServer;
        ParseResultCollection ParsePlaceName(string text)
        {
            ParserContext context = new ParserContext(dictAddr);
            context.Text = text;
            return ParseResultCollection.InternalParse(text, new PlaceNameParser(context));
        }

        [TestMethod]
        public void TestParsePlaceName()
        {
            IParser p = new PlaceNameParser(GeneralParserTest.CreateParserContext("你好，我在上海，他在北京。", dictAddr));
            ParseResultCollection prc = p.Parse(0);
            Assert.AreEqual(0, prc.Count);

            prc = p.Parse(5);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "上海", 5, POSType.A_NS);

            prc = p.Parse(10);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "北京", 10, POSType.A_NS);
        }
        [TestMethod]
        public void TestParseFullSentenceForPlaceNames()
        {
            ParseResultCollection prc = ParsePlaceName("沈阳的天气真好，如果哪天我能去沈阳玩就好了。呼和浩特那里咋样？");
            Assert.AreEqual(3, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "沈阳", 0, POSType.A_NS);
            GeneralParserTest.AssertParseResult(prc[1], "沈阳", 15, POSType.A_NS);
            GeneralParserTest.AssertParseResult(prc[2], "呼和浩特", 22, POSType.A_NS);

            ParseResultCollection prc2 = ParsePlaceName("传统“金砖四国”（BRIC）引用了巴西、俄罗斯、印度和中国的英文首字母。");
            Assert.AreEqual(4, prc2.Count);
            GeneralParserTest.AssertParseResult(prc2[0], "巴西", 17, POSType.A_NS);
            GeneralParserTest.AssertParseResult(prc2[1], "俄罗斯", 20, POSType.A_NS);
            GeneralParserTest.AssertParseResult(prc2[2], "印度", 24, POSType.A_NS);
            GeneralParserTest.AssertParseResult(prc2[3], "中国", 27, POSType.A_NS);
        }
    }
}
