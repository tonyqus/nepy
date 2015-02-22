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
    public class SimChineseQuantityParserTest
    {
        ParseResultCollection ParseChineseQuantity(string text)
        {
            ParserContext context = new ParserContext();
            context.Text = text;
            return ParseResultCollection.InternalParse(text, new SimChineseQuantityParser(context));
        }
        [Test]
        public void TestParseSingleQuantity()
        {
            IParser p = new SimChineseQuantityParser(TestUtility.CreateParserContext("一棵树通常有二十五根树杈。"));
            ParseResultCollection prc = p.Parse(0);
            Assert.AreEqual(2, prc.Count);
            TestUtility.AssertParseResult(prc[0], "一", 0, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "棵", 1, POSType.A_Q);

            prc = p.Parse(6);
            Assert.AreEqual(2, prc.Count);
            TestUtility.AssertParseResult(prc[0], "二十五", 6, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "根", 9, POSType.A_Q);
        }

        [Test]
        public void TestParseFullSentenceForQuantity()
        {
            ParseResultCollection prc = ParseChineseQuantity("常用量词：一台机器 一部字典 一份杂志 十五台电脑一方手帕 一颗黄豆 10台洗衣机");
            Assert.AreEqual(14, prc.Count);
            TestUtility.AssertParseResult(prc[0], "一", 5, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "台", 6, POSType.A_Q);
            TestUtility.AssertParseResult(prc[2], "一", 10, POSType.A_M);
            TestUtility.AssertParseResult(prc[3], "部", 11, POSType.A_Q);
            TestUtility.AssertParseResult(prc[4], "一", 15, POSType.A_M);
            TestUtility.AssertParseResult(prc[5], "份", 16, POSType.A_Q);
            TestUtility.AssertParseResult(prc[6], "十五", 20, POSType.A_M);
            TestUtility.AssertParseResult(prc[7], "台", 22, POSType.A_Q);

            TestUtility.AssertParseResult(prc[8], "一", 25, POSType.A_M);
            TestUtility.AssertParseResult(prc[9], "方", 26, POSType.A_Q);
            TestUtility.AssertParseResult(prc[10], "一", 30, POSType.A_M);
            TestUtility.AssertParseResult(prc[11], "颗", 31, POSType.A_Q);
            TestUtility.AssertParseResult(prc[12], "10", 35, POSType.A_M);
            TestUtility.AssertParseResult(prc[13], "台", 37, POSType.A_Q);
        }
        [Test]
        public void TestParseQuantityFromIKAnalyzer()
        {
            ParseResultCollection prc = ParseChineseQuantity("1500名常用的数量和人名的匹配超过22万个");
            Assert.AreEqual(4, prc.Count);
            TestUtility.AssertParseResult(prc[0], "1500", 0, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "名", 4, POSType.A_Q);
            TestUtility.AssertParseResult(prc[2], "22万", 18, POSType.A_M);
            TestUtility.AssertParseResult(prc[3], "个", 21, POSType.A_Q);

            prc = ParseChineseQuantity("这个月我们家用了一百五十三度电");
            Assert.AreEqual(2, prc.Count);
            TestUtility.AssertParseResult(prc[0], "一百五十三", 8, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "度", 13, POSType.A_Q);

            prc = ParseChineseQuantity("欢迎使用阿江统计2.01版");
            Assert.AreEqual(2, prc.Count);
            TestUtility.AssertParseResult(prc[0], "2.01", 8, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "版", 12, POSType.A_Q);

            prc = ParseChineseQuantity("千年等一回");
            Assert.AreEqual(2, prc.Count);
            TestUtility.AssertParseResult(prc[0], "一", 3, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "回", 4, POSType.A_Q);

            prc = ParseChineseQuantity("关羽真是一员猛将");
            Assert.AreEqual(2, prc.Count);
            TestUtility.AssertParseResult(prc[0], "一", 4, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "员", 5, POSType.A_Q);

            prc = ParseChineseQuantity("为了这三世姻缘");
            Assert.AreEqual(2, prc.Count);
            TestUtility.AssertParseResult(prc[0], "三", 3, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "世", 4, POSType.A_Q);

            prc = ParseChineseQuantity("鲁智深一拳打死郑关西，最后还踹了他一脚");
            Assert.AreEqual(4, prc.Count);
            TestUtility.AssertParseResult(prc[0], "一", 3, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "拳", 4, POSType.A_Q);
            TestUtility.AssertParseResult(prc[2], "一", 17, POSType.A_M);
            TestUtility.AssertParseResult(prc[3], "脚", 18, POSType.A_Q);

            prc = ParseChineseQuantity("他家有15亩农田");
            Assert.AreEqual(2, prc.Count);
            TestUtility.AssertParseResult(prc[0], "15", 3, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "亩", 5, POSType.A_Q);

            prc = ParseChineseQuantity("这个文件总共有15.3兆");
            Assert.AreEqual(2, prc.Count);
            TestUtility.AssertParseResult(prc[0], "15.3", 7, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "兆", 11, POSType.A_Q);

            prc = ParseChineseQuantity("一元钱能干吗");
            Assert.AreEqual(2, prc.Count);
            TestUtility.AssertParseResult(prc[0], "一", 0, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "元", 1, POSType.A_Q);


            prc = ParseChineseQuantity("经历了两届奥运会");
            Assert.AreEqual(2, prc.Count);
            TestUtility.AssertParseResult(prc[0], "两",3, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "届",4, POSType.A_Q);       
        }
        [Test]
        public void TestNotZhangSanCase()
        {
            ParseResultCollection prc = ParseChineseQuantity("李三买了一张三角桌子");
            Assert.AreEqual(4, prc.Count);
            TestUtility.AssertParseResult(prc[0], "一", 4, POSType.A_M);
            TestUtility.AssertParseResult(prc[1], "张", 5, POSType.A_Q);
            TestUtility.AssertParseResult(prc[2], "三", 6, POSType.A_M);   //TODO: 是否要移除 三角
            TestUtility.AssertParseResult(prc[3], "角", 7, POSType.A_Q);
        }
        [Test]
        public void TestInvalidQuantityCase()
        {
            IParser p = new SimChineseQuantityParser(TestUtility.CreateParserContext("兆棵树"));
            ParseResultCollection prc = p.Parse(0);
            Assert.AreEqual(0, prc.Count);
        }
    }
}
