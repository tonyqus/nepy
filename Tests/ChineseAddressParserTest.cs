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
    public class ChineseAddressParserTest
    {
        public void AssertAddressValue(ChineseAddress ca, string province, string city, string district, string street, string no, string floor, string room)
        {
            AssertAddressValue(ca, null, province, city, district, street, no, floor, room,null);
        }
        public void AssertAddressValue(ChineseAddress ca, string country, string province, string city, string district, string street, string no, string floor, string room, string building)
        {
            AssertAddressValue(ca, country, province, city, district, street, no, floor, room,building,null,null);
        }
        public void AssertAddressValue(ChineseAddress ca,string country, string province, string city, string district, string street, string no, string floor, string room, string building,string extra,string lane)
        {
            Assert.AreEqual(province, ca.province);
            Assert.AreEqual(district, ca.district);
            Assert.AreEqual(city, ca.city);
            Assert.AreEqual(street, ca.street);
            Assert.AreEqual(no, ca.no);
            Assert.AreEqual(floor, ca.floor);
            Assert.AreEqual(room, ca.room);
            Assert.AreEqual(lane, ca.lane);
            Assert.AreEqual(country, ca.country);
            Assert.AreEqual(building, ca.building);
        }

        const string dictAddr = TestConfig.dictServer;

        ParserContext CreateParserContext(string text)
        {
            return GeneralParserTest.CreateParserContext(text, dictAddr);
        }
        [TestMethod]
        public void TestParseSingleAddressWithDuplicateCity()
        { 
                IParser parser = new ChineseAddressParser(CreateParserContext("上海市上海市黄浦区外马路1410号"));
                ParseResultCollection prc = parser.Parse(0);
                Assert.AreEqual(1, prc.Count);
                GeneralParserTest.AssertParseResult(prc[0], "上海市上海市黄浦区外马路1410号", 0, POSType.D_S);
                AssertAddressValue((ChineseAddress)prc[0].Value, null,"上海市", "黄浦区", "外马路", "1410号",null,null);

                parser = new ChineseAddressParser(CreateParserContext("上海上海市黄浦区外马路1410号"));
                prc = parser.Parse(0);
                Assert.AreEqual(1, prc.Count);
                GeneralParserTest.AssertParseResult(prc[0], "上海黄浦区外马路1410号", 0, POSType.D_S);
                AssertAddressValue((ChineseAddress)prc[0].Value, null, "上海", "黄浦区", "外马路", "1410号", null, null);    //出现两个上海时，以第一个为准
        }
        [TestMethod]
        public void TestParseSingleSpecialAddress()
        {
                IParser parser = new ChineseAddressParser(CreateParserContext("地址：杭州市江干区九堡九环路60号一号厂房"));
                ParseResultCollection prc = parser.Parse(3);
                Assert.AreEqual(1, prc.Count);
                GeneralParserTest.AssertParseResult(prc[0], "杭州市江干区九堡九环路60号一号", 3, POSType.D_S);    //TODO: 厂房无法识别
        }
        [TestMethod]
        public void TestParseSingleAddressWithoutTerminator()
        {
                IParser parser = new ChineseAddressParser(CreateParserContext("上海市黄浦区内环南浦大桥立交桥"));
                ParseResultCollection prc = parser.Parse(0);
                Assert.AreEqual(1, prc.Count);
                GeneralParserTest.AssertParseResult(prc[0], "上海市黄浦区内环南浦大桥", 0, POSType.D_S);
                AssertAddressValue((ChineseAddress)prc[0].Value, null, null, "上海市", "黄浦区", null, null, null, null, null, "内环南浦大桥",null); //TODO: 立交桥

                parser = new ChineseAddressParser(CreateParserContext("地址：杭州市江干区九堡九环路60号（厂房）"));
                prc = parser.Parse(3);
                Assert.AreEqual(1, prc.Count);
                GeneralParserTest.AssertParseResult(prc[0], "杭州市江干区九堡九环路60号（厂房）", 3, POSType.D_S);
                AssertAddressValue((ChineseAddress)prc[0].Value, null, null, "杭州市", "江干区", "九堡九环路", "60号", null, null, null, "（厂房）",null);

                parser = new ChineseAddressParser(CreateParserContext("杭州市江干区九堡九环路60号（厂房） 邮编："));
                prc = parser.Parse(0);
                Assert.AreEqual(1, prc.Count);
                GeneralParserTest.AssertParseResult(prc[0], "杭州市江干区九堡九环路60号（厂房）", 0, POSType.D_S);
                AssertAddressValue((ChineseAddress)prc[0].Value, null, null, "杭州市", "江干区", "九堡九环路", "60号", null, null, null, "（厂房）",null);

                parser = new ChineseAddressParser(CreateParserContext("地址：杭州红楼大酒店二楼融府中餐厅"));
                prc = parser.Parse(3);
                Assert.AreEqual(1, prc.Count);
                GeneralParserTest.AssertParseResult(prc[0], "杭州红楼大酒店二楼融府中餐厅", 3, POSType.D_S);
                AssertAddressValue((ChineseAddress)prc[0].Value, null, null, "杭州", null, null, null, "二楼", null, "红楼大酒店", "融府中餐厅",null);

                parser = new ChineseAddressParser(CreateParserContext("浦东新区红楼大酒店三楼"));
                prc = parser.Parse(0);
                GeneralParserTest.AssertParseResult(prc[0], "浦东新区红楼大酒店三楼", 0, POSType.D_S);
                AssertAddressValue((ChineseAddress)prc[0].Value, null, null, null, "浦东新区", null, null, "三楼", null, "红楼大酒店", null,null);
        }

        [TestMethod]
        public void TestParseSingleAddress()
        {
            IParser parser = new ChineseAddressParser(CreateParserContext("上海市黄浦区外马路1410号"));
            ParseResultCollection prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0],"上海市黄浦区外马路1410号", 0,POSType.D_S);
            AssertAddressValue((ChineseAddress)prc[0].Value, null,"上海市", "黄浦区", "外马路", "1410号",null,null);


            parser = new ChineseAddressParser(CreateParserContext("上海市黄浦区陆家浜路413弄5号702室（金南新苑商务楼）"));
            prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "上海市黄浦区陆家浜路413弄5号702室（金南新苑商务楼）", 0, POSType.D_S);
            AssertAddressValue((ChineseAddress)prc[0].Value, null, null, "上海市", "黄浦区", "陆家浜路", "5号", null, "702室",null,null,"413弄");


            parser = new ChineseAddressParser(CreateParserContext("中国上海市浦东新区银城中路68号时代金融中心大厦38楼"));
            prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "中国上海市浦东新区银城中路68号时代金融中心大厦38楼", 0, POSType.D_S);
            AssertAddressValue((ChineseAddress)prc[0].Value, "中国", null, "上海市", "浦东新区", "银城中路", "68号", "38楼", null, "时代金融中心大厦");

            parser = new ChineseAddressParser(CreateParserContext("杭州市体育场路453号14楼302室"));
            prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "杭州市体育场路453号14楼302室", 0, POSType.D_S);
            AssertAddressValue((ChineseAddress)prc[0].Value, null, "杭州市", null, "体育场路", "453号", "14楼", "302室");

            parser = new ChineseAddressParser(CreateParserContext("地址：杭州市江干区九堡九环路六十号"));
            prc = parser.Parse(3);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "杭州市江干区九堡九环路六十号", 3, POSType.D_S);
            AssertAddressValue((ChineseAddress)prc[0].Value, null, "杭州市", "江干区", "九堡九环路", "六十号", null,null);

            parser = new ChineseAddressParser(CreateParserContext("杭州西湖区文二西路2号"));
            prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "杭州西湖区文二西路2号", 0, POSType.D_S);
            AssertAddressValue((ChineseAddress)prc[0].Value, null, "杭州", "西湖区", "文二西路", "2号", null, null);

            parser = new ChineseAddressParser(CreateParserContext("长乐路460弄10号"));
            prc = parser.Parse(0);
            Assert.AreEqual(1, prc.Count);
            GeneralParserTest.AssertParseResult(prc[0], "长乐路460弄10号", 0, POSType.D_S);
            AssertAddressValue((ChineseAddress)prc[0].Value, null,null,null,null, "长乐路",  "10号",null, null, null,null,"460弄");
        }

        [TestMethod]
        public void TestParseNoneAddressText()
        {
            IParser parser = new ChineseAddressParser(CreateParserContext("这是一个测试"));
            ParseResultCollection prc = parser.Parse(0);
            Assert.AreEqual(0, prc.Count);          
            
        }
    }
}
