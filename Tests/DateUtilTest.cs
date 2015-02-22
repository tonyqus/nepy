using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Nepy.Core;
using Nepy.Parsers;
using NUnit.Framework;


namespace Nepy.Tests
{
    [TestFixture]
    public class DateUtilTest
    {
        [Test]
        public void TestConvertChineseText2DateTime()
        {
            Assert.AreEqual(
                DateUtil.ConvertChineseText2DateText("2005年至今", "yyyy年"),
                "[2005年]至今");
            Assert.AreEqual(
                DateUtil.ConvertChineseText2DateText("2005年7月至今", "yyyy年M月"),
                "[2005年7月]至今");
            Assert.AreEqual(
                DateUtil.ConvertChineseText2DateText("2005年7月1到2010年5月", "yyyy年M月"),
                "[err]到[2010年5月]");
            Assert.AreEqual(
                DateUtil.ConvertChineseText2DateText("2005年7月 - 2010 年 05月", "yyyy年MM月"),
                "[2005年07月]-[2010年05月]");
        }

        ParseResultCollection ParseChineseDateTime(string text)
        {
            ParserContext context = new ParserContext();
            context.Text = text;
            return ParseResultCollection.InternalParse(text, new SimChineseDateTimeParser(context));
        }
        [Test]
        public void TestChineseNonNumericDateText()
        {
            ParseResultCollection prList = null;
            prList = ParseChineseDateTime("今天是个好日子。");
            Assert.AreEqual(1, prList.Count);
            ParseResult pr = prList[0];
            Assert.AreEqual("今天", pr.Text);
            Assert.AreEqual(2, pr.Length);
            Assert.AreEqual(0, pr.StartPos);
            Assert.AreEqual(POSType.D_T, pr.Type);
            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //Assert.AreEqual(now, (DateTime)pr.Value);

            prList = ParseChineseDateTime("你好，明天你会和昨天一样来吗？");
            Assert.AreEqual(2, prList.Count);
            pr = prList[0];
            Assert.AreEqual("明天", pr.Text);
            Assert.AreEqual(2, pr.Length);
            Assert.AreEqual(3, pr.StartPos);
            Assert.AreEqual(POSType.D_T, pr.Type);
            //Assert.AreEqual(now.AddDays(1), (DateTime)pr.Value);

            pr = prList[1];
            Assert.AreEqual("昨天", pr.Text);
            Assert.AreEqual(2, pr.Length);
            Assert.AreEqual(8, pr.StartPos);
            Assert.AreEqual(POSType.D_T, pr.Type);
            //Assert.AreEqual(now.AddDays(-1), (DateTime)pr.Value);

            prList = ParseChineseDateTime("前天是2012年1月10日，晴");
            Assert.AreEqual(2, prList.Count);
            pr = prList[0];
            Assert.AreEqual("前天", pr.Text);
            Assert.AreEqual(2, pr.Length);
            Assert.AreEqual(0, pr.StartPos);
            Assert.AreEqual(POSType.D_T, pr.Type);

            pr = prList[1];
            Assert.AreEqual("2012年1月10日", pr.Text);
            Assert.AreEqual(10, pr.Length);
            Assert.AreEqual(3, pr.StartPos);
            Assert.AreEqual(POSType.D_T, pr.Type);
            Assert.AreEqual(new DateTime(2012, 1, 10), (DateTime)pr.Value);
        }

        [Test]
        public void TestConvertChineseText2Result()
        {
            ParseResultCollection prList = null;
            prList = ParseChineseDateTime("2005年至今");
            Assert.AreEqual(1, prList.Count);
            ParseResult pr = prList[0];
            Assert.AreEqual("2005年", pr.Text);
            Assert.AreEqual(5, pr.Length);
            Assert.AreEqual(0, pr.StartPos);
            Assert.AreEqual(POSType.D_T, pr.Type);
            Assert.AreEqual(new DateTime(2005, 1, 1), (DateTime)pr.Value);

            prList.Clear();
            prList = ParseChineseDateTime("2005年7月至今");
            Assert.AreEqual(1, prList.Count);
            pr = prList[0];
            Assert.AreEqual("2005年7月", pr.Text);
            Assert.AreEqual(7, pr.Length);
            Assert.AreEqual(0, pr.StartPos);
            Assert.AreEqual(POSType.D_T, pr.Type);
            Assert.AreEqual(new DateTime(2005, 7, 1), (DateTime)pr.Value);

            prList.Clear();
            prList = ParseChineseDateTime("2005年7月2日到2010年5月");
            Assert.AreEqual(2, prList.Count);
            ParseResult pr0 = prList[0];
            Assert.AreEqual("2005年7月2日", pr0.Text);
            Assert.AreEqual(9, pr0.Length);
            Assert.AreEqual(0, pr0.StartPos);
            Assert.AreEqual(POSType.D_T, pr0.Type);
            Assert.AreEqual((DateTime)pr0.Value, new DateTime(2005, 7, 2));

            ParseResult pr1 = prList[1];
            Assert.AreEqual("2010年5月", pr1.Text);
            Assert.AreEqual(7, pr1.Length);
            Assert.AreEqual(10, pr1.StartPos);
            Assert.AreEqual(POSType.D_T, pr1.Type);
            Assert.AreEqual((DateTime)pr1.Value, new DateTime(2010, 5, 1));

            prList.Clear();
            prList = ParseChineseDateTime("2005年7月 - 2010 年 05月");
            Assert.AreEqual(2, prList.Count);
            pr0 = prList[0];
            Assert.AreEqual("2005年7月", pr0.Text);
            Assert.AreEqual(7, pr0.Length);
            Assert.AreEqual(0, pr0.StartPos);
            Assert.AreEqual(POSType.D_T, pr0.Type);
            Assert.AreEqual((DateTime)pr0.Value, new DateTime(2005, 7, 1));

            pr1 = prList[1];
            Assert.AreEqual("2010 年 05月", pr1.Text);
            Assert.AreEqual(10, pr1.Length);
            Assert.AreEqual(9, pr1.StartPos);
            Assert.AreEqual(POSType.D_T, pr1.Type);
            Assert.AreEqual((DateTime)pr1.Value, new DateTime(2010, 5, 1));
        }
        [Test]
        public void TestDatePatternInEducationInfoFrom51job()
        {
            ParseResultCollection prc = null;
            prc = ParseChineseDateTime("2003年04月 -- 2005年04月：东京都江湖文化中心");
            Assert.AreEqual(2, prc.Count);
            ParseResult pr;
            pr = prc[0];
            Assert.AreEqual("2003年04月", pr.Text);
            Assert.AreEqual(8, pr.Length);
            Assert.AreEqual(0, pr.StartPos);
            Assert.AreEqual(POSType.D_T, pr.Type);
            Assert.AreEqual((DateTime)pr.Value, new DateTime(2003, 4, 1));
            pr = prc[1];
            Assert.AreEqual("2005年04月", pr.Text);
            Assert.AreEqual(8, pr.Length);
            Assert.AreEqual(11, pr.StartPos);
            Assert.AreEqual(POSType.D_T, pr.Type);
            Assert.AreEqual((DateTime)pr.Value, new DateTime(2005, 4, 1));
        }
        [Test]
        public void TestConvertDateTextFrom51Job()
        {
            Assert.AreEqual("[2011/09]--至今",
                DateUtil.ConvertChineseText2DateText("2011/09 -- 至今", "yyyy/MM")
                );


        }
        //[Test]
        //public void TestConvertDateTextFrom51Job2()
        //{
        //    Assert.AreEqual("2008.11 - 2011.04",
        //        DateUtil.ConvertChineseText2DateText("2011/09 -- 至今", "yyyy/MM")
        //        );
        //}
    }
}
