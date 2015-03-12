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
    public class OrgNamePraserTest
    {

        const string dictAddr = TestConfig.dictServer;
        ParseResultCollection ParseChineseOrgName(string text)
        {
            ParserContext context = new ParserContext(dictAddr);
            context.Text = text;
            return ParseResultCollection.InternalParse(text, new OrgNameParser(context));
        }
        [TestMethod]
        public void TestParseSingleOrgName()
        {
            IParser p = new OrgNameParser(GeneralParserTest.CreateParserContext("上海软星动力软件有限公司", dictAddr));
            ParseResultCollection prc = p.Parse(0);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "上海软星动力软件有限公司", 0, POSType.A_NT);

            p = new OrgNameParser(GeneralParserTest.CreateParserContext("这里是上海互联网信息有限公司", dictAddr));
            prc = p.Parse(3);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "上海互联网信息有限公司", 3, POSType.A_NT);

            p = new OrgNameParser(GeneralParserTest.CreateParserContext("鹿特丹美术学院", dictAddr));
            prc = p.Parse(0);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "鹿特丹美术学院", 0, POSType.A_NT);

            p = new OrgNameParser(GeneralParserTest.CreateParserContext("鹿特丹美院", dictAddr));
            prc = p.Parse(0);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "鹿特丹美院", 0, POSType.A_NT);

            p = new OrgNameParser(GeneralParserTest.CreateParserContext("先锋商泰（上海）有限公司", dictAddr));
            prc = p.Parse(0);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "先锋商泰（上海）有限公司", 0, POSType.A_NT);
        }
        [TestMethod]
        public void TestParseFullSentenceForOrgName()
        {
            ParseResultCollection prc = ParseChineseOrgName("郑荣科 07行政管理专业 现任职于上海江正营销策划有限公司");
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "上海江正营销策划有限公司", 17, POSType.A_NT);

            prc = ParseChineseOrgName("李承龙 08民航 现任职于中国南方国际航空。");
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "中国南方国际航空", 13, POSType.A_NT);

            prc = ParseChineseOrgName("李林玲（左） 06旅游管理班 现任职于上海中福大酒店。");
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "上海中福大酒店", 19, POSType.A_NT);
        }
        [TestMethod]
        public void TestParseFullSentenceForSchool()
        {
            ParseResultCollection prc = ParseChineseOrgName("09/01/1996 – 07/01/1999  安徽省肥东第二中学高中部学习");
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "安徽省肥东第二中学", 25, POSType.A_NT);

            prc = ParseChineseOrgName("09/01/1999 – 07/01/2003  安徽大学数学系数学与应用数学专业学习");
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "安徽大学", 25, POSType.A_NT);

            prc = ParseChineseOrgName("09/01/1999 – 07/01/2003  清华大学数学系");
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "清华大学", 25, POSType.A_NT);

            prc = ParseChineseOrgName("北京大学、清华大学和中国人民大学高居2008中国最受媒体关注大学排行榜前三强");
            Assert.AreEqual(4, prc.Count);  
            GeneralParserTest.AssertParseResult(prc[0], "北京大学", 0, POSType.A_NT);
            GeneralParserTest.AssertParseResult(prc[1], "清华大学", 5, POSType.A_NT);
            GeneralParserTest.AssertParseResult(prc[2], "中国人民大学", 10, POSType.A_NT);
            GeneralParserTest.AssertParseResult(prc[3], "中国最受媒体关注大学", 22, POSType.A_NT);            
        }

        [TestMethod]
        public void TestParseFullSentenceForPublisherName()
        {
            ParseResultCollection prc = ParseChineseOrgName("《软件测试方法与技术实践指南Java EE版》已经由清华大学出版社出版");
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "清华大学出版社", 26, POSType.A_NT);

            prc = ParseChineseOrgName("作为法律出版社新书、重点书发布平台，我们致力于为读者提供优质的信息服务。");
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "法律出版社", 2, POSType.A_NT);
        }
    }
}
