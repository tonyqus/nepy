
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Nepy.Core;

namespace Nepy.Tests
{
    [TestFixture]
    public class UnicodeBlockTest
    {
        [Test]
        public void TestOfMethod()
        {            
            Assert.AreEqual(UnicodeBlock.CJK_UNIFIED_IDEOGRAPHS,UnicodeBlock.Of('中'));
            Assert.AreEqual(UnicodeBlock.CJK_UNIFIED_IDEOGRAPHS, UnicodeBlock.Of('語'));
            Assert.AreEqual(UnicodeBlock.HANGUL_SYLLABLES, UnicodeBlock.Of('한'));
            Assert.AreEqual(UnicodeBlock.BASIC_LATIN, UnicodeBlock.Of('E'));
            Assert.AreEqual(UnicodeBlock.BASIC_LATIN, UnicodeBlock.Of('a'));
            Assert.AreEqual(UnicodeBlock.BASIC_LATIN, UnicodeBlock.Of('$'));
            Assert.AreEqual(UnicodeBlock.BASIC_LATIN, UnicodeBlock.Of('-'));
            Assert.AreEqual(UnicodeBlock.LATIN_1_SUPPLEMENT, UnicodeBlock.Of('ç'));
            Assert.AreEqual(UnicodeBlock.LATIN_1_SUPPLEMENT, UnicodeBlock.Of('ñ'));
            Assert.AreEqual(UnicodeBlock.GREEK, UnicodeBlock.Of('ω'));
            Assert.AreEqual(UnicodeBlock.CYRILLIC, UnicodeBlock.Of('Ѧ'));            
        }
        [Test]
        public void TestForName()
        {
            Assert.AreEqual(UnicodeBlock.BASIC_LATIN, UnicodeBlock.ForName("Basic Latin"));
            Assert.AreEqual(UnicodeBlock.HANGUL_SYLLABLES, UnicodeBlock.ForName("HANGUL_SYLLABLES"));
            try
            {
                UnicodeBlock.ForName("Basic___ Latin");
                Assert.Fail("exception should be thrown");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("invalid block name", e.Message);
            }
        }
    }
}
