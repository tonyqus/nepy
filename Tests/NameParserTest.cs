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
    public class NameParserTest
    {
        const string dictServer = TestConfig.dictServer;
        [TestMethod]
        public void TestParseSingleName()
        {
            IParser p = new NameParser(GeneralParserTest.CreateParserContext("你好，我叫毛泽东，这位是朱德同志。", dictServer));
            ParseResultCollection prc = p.Parse(5);
            Assert.AreEqual(2, prc.Count);

            GeneralParserTest.AssertParseResult(prc[0], "毛", 5, POSType.A_NR);
            GeneralParserTest.AssertParseResult(prc[1], "泽东", 6, POSType.A_NR);

            prc = p.Parse(12);
            Assert.AreEqual(3, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "朱", 12, POSType.A_NR);
            GeneralParserTest.AssertParseResult(prc[1], "德", 13, POSType.A_NR);
            GeneralParserTest.AssertParseResult(prc[2], "同志", 14, POSType.D_N);
        }
        [TestMethod]
        public void TestParseNameWithPrefix()
        {
            IParser p = new NameParser(GeneralParserTest.CreateParserContext("老张说这里是我们的地盘", dictServer));
            ParseResultCollection prc = p.Parse(0);
            Assert.AreEqual(2, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "老", 0, POSType.D_N);
            GeneralParserTest.AssertParseResult(prc[1], "张", 1, POSType.A_NR);

            p = new NameParser(GeneralParserTest.CreateParserContext("劳动模范代表李素丽", dictServer));
            prc = p.Parse(4);
            Assert.AreEqual(3, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "代表", 4, POSType.D_N);
            GeneralParserTest.AssertParseResult(prc[1], "李", 6, POSType.A_NR);
            GeneralParserTest.AssertParseResult(prc[2], "素丽", 7, POSType.A_NR);

            p = new NameParser(GeneralParserTest.CreateParserContext("国务院副总理钱其琛向全国人民", dictServer));
            prc = p.Parse(4);
            Assert.AreEqual(3, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "总理", 4, POSType.D_N);
            GeneralParserTest.AssertParseResult(prc[1], "钱", 6, POSType.A_NR);
            GeneralParserTest.AssertParseResult(prc[2], "其琛", 7, POSType.A_NR);
        }
        [TestMethod]
        public void TestParseNameWithSuffix()
        {
            IParser p = new NameParser(GeneralParserTest.CreateParserContext("王教授给我们授课", dictServer));
            ParseResultCollection prc = p.Parse(0);
            Assert.AreEqual(2, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "王", 0, POSType.A_NR);
            GeneralParserTest.AssertParseResult(prc[1], "教授", 1, POSType.D_N);


            p = new NameParser(GeneralParserTest.CreateParserContext("王琪斌教授给我们授课", dictServer));
            prc = p.Parse(0);
            Assert.AreEqual(3, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "王", 0, POSType.A_NR);
            GeneralParserTest.AssertParseResult(prc[1], "琪斌", 1, POSType.A_NR);
            GeneralParserTest.AssertParseResult(prc[2], "教授", 3, POSType.D_N);
        }
        [TestMethod]
        public void TestParseNotZhangSanCase()
        {
           IParser p = new NameParser(GeneralParserTest.CreateParserContext("李三买了一张三角桌子", dictServer));
           ParseResultCollection prc = p.Parse(0);
           Assert.AreEqual(2, prc.Count);

           GeneralParserTest.AssertParseResult(prc[0], "李", 0, POSType.A_NR);
           GeneralParserTest.AssertParseResult(prc[1], "三", 1, POSType.A_NR);
           
           prc = p.Parse(5);
           Assert.AreEqual(0, prc.Count); 
        }
        [TestMethod]
        public void TestNameWithPunctuation()
        {
            IParser p = new NameParser(GeneralParserTest.CreateParserContext("（记者李术峰）", dictServer));
           ParseResultCollection prc = p.Parse(1);
           Assert.AreEqual(3, prc.Count);
           GeneralParserTest.AssertParseResult(prc[0], "记者", 1, POSType.D_N);
           GeneralParserTest.AssertParseResult(prc[1], "李", 3, POSType.A_NR);
           GeneralParserTest.AssertParseResult(prc[2], "术峰", 4, POSType.A_NR);

           p = new NameParser(GeneralParserTest.CreateParserContext("张，", dictServer));
           prc = p.Parse(0);
           Assert.AreEqual(0, prc.Count);

           p = new NameParser(GeneralParserTest.CreateParserContext("（记者李术峰 ）", dictServer));
           prc = p.Parse(1);
           Assert.AreEqual(3, prc.Count);

           GeneralParserTest.AssertParseResult(prc[0], "记者", 1, POSType.D_N);
           GeneralParserTest.AssertParseResult(prc[1], "李", 3, POSType.A_NR);
           GeneralParserTest.AssertParseResult(prc[2], "术峰", 4, POSType.A_NR);
        }
        
    }
}
