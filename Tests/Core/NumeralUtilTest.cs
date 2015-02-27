using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Nepy.Core;
using NUnit.Framework;

namespace Nepy.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestFixture]
    public class NumeralUtilTest
    {
        public NumeralUtilTest()
        {
        }
        [Test]
        public void TestConvertChineseNumeral2Arabic()
        {
            Assert.AreEqual(
                NumeralUtil.ConvertChineseNumeral2Arabic("你好，这里有二百三十五块钱，收好了，总共二千二百五十二块。二减五等于负三万零五十。"),
                "你好，这里有235块钱，收好了，总共2252块。2减5等于-30050。"
                );

            Assert.AreEqual(
                NumeralUtil.ConvertChineseNumeral2Arabic("公元二零零五年四月"),
                "公元2005年4月"                
                );
            Assert.AreEqual(
                NumeralUtil.ConvertChineseNumeral2Arabic("公元前四五五年"),
                "公元前455年"
                );
        }



        [Test]
        public void TestIsChineseNumeral()
        { 
            char[] chnGenText = new char[] { '零', '一', '二', '三', '四', '五', '六', '七','八', '九' };
            
            foreach(char ch in chnGenText)
            {
                Assert.IsTrue(NumeralUtil.IsChineseNumeral(ch));
            }
            char[] chnRMBText = new char[] { '零', '壹', '贰', '叁', '肆', '伍', '陆', '染', '捌', '玖' };
            foreach(char ch in chnRMBText)
            {
                Assert.IsTrue(NumeralUtil.IsChineseNumeral(ch));
            }
            Assert.IsFalse(NumeralUtil.IsChineseNumeral('7'));
        }

        [Test]
        public void TestIsEnglishNumeral()
        {
            string[] enNumText = new string[] { "one", "two", "three", "four", "five", "six", "steven", "eight", "nine", "ten", 
                "billion","million","thousand" };

            foreach (string str in enNumText)
            {
                Assert.IsTrue(NumeralUtil.IsEnglishNumeral(str),str);
            }
        }
        [Test]
        public void TestCompareWithWhitespace()
        {
            Assert.IsTrue(Utility.CompareWithWhitespace("Hello World","Hello World"));
            Assert.IsTrue(Utility.CompareWithWhitespace("Hello World", "Hello  World "));
            Assert.IsFalse(Utility.CompareWithWhitespace("Hello World", "Hello World!"));
            Assert.IsTrue(Utility.CompareWithWhitespace("姓名", "姓 名"));
            Assert.IsTrue(Utility.CompareWithWhitespace("姓　名", "姓 名"));
        }
    }
}
