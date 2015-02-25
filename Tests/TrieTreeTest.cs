using Moq;
using Nepy.Core;
using Nepy.Dictionary;
using Nepy.Dictionary.Providers;
using NUnit.Framework;

namespace DictionaryService.UnitTest
{
    [TestFixture]
    public class TrieTreeTest
    {
        [Test]
        public void TestCreateBasicTrieTree()
        {
            TrieTree tt = TrieTree.GetInstance();
            tt.AddWord("测试");
            tt.AddWord("你好");
            tt.AddWord("我们");
            tt.AddWord("你们");

            Assert.AreEqual(3, tt.Root.Children.Count);
            TrieTreeNode tn1 = tt.GetNode("你们");
            Assert.IsNotNull(tn1);
            Assert.AreEqual("你们", tn1.Word);
            Assert.IsTrue(tn1.Children.Count==0);
            Assert.IsNull(tt.GetNode("它们"));

            TrieTreeNode tn=tt.GetNode('你');
            Assert.AreEqual("你",tn.Word);
            Assert.AreEqual(2, tn.Children.Count);

            Assert.AreEqual(1, tt.GetNode("我").Children.Count);
        }
        [Test]
        public void TestCreateBasicTrieTreeWithPOSType()
        {
            TrieTree tt = TrieTree.GetInstance();
            tt.AddWord("测试", POSType.D_N);
            tt.AddWord("你好", POSType.UNKNOWN);
            tt.AddWord("我们", POSType.D_R);
            tt.AddWord("你们", POSType.D_R);

            Assert.AreEqual(3, tt.Root.Children.Count);
            TrieTreeNode tn1 = tt.GetNode("你们", (int)POSType.D_N);
            Assert.IsNull(tn1);
            TrieTreeNode tn2 = tt.GetNode("你们", (int)POSType.D_R);
            Assert.IsNotNull(tn2);
            Assert.AreEqual("你们", tn2.Word);
            Assert.IsTrue(tn2.Children.Count == 0);

            Assert.IsNull(tt.GetNode("它们"));

            TrieTreeNode tn = tt.GetNode('你');
            Assert.AreEqual("你", tn.Word);
            Assert.AreEqual(2, tn.Children.Count);

            tn = tt.GetNode('你', (int)POSType.UNKNOWN);
            Assert.AreEqual("你", tn.Word);

            Assert.AreEqual(1, tt.GetNode("我").Children.Count);
        }
        [Test]
        public void TestLoadTrieTreeFromPanguDict()
        {
            var mockPanguProviderSetting = new Mock<IDataProviderSetting>();
            mockPanguProviderSetting.Setup(s => s.Uri).Returns(DictLoadTest.UnitTestProjectFolder + @"\Data\panguDict.dct");
            mockPanguProviderSetting.Setup(s => s.ProviderType).Returns("PanguDictProvider");

            var panguProvider = new PanguDictProvider(mockPanguProviderSetting.Object);
            TrieTree tt = TrieFactory.LoadFromDataProvider(panguProvider);
            TrieTreeNode ttn = tt.GetNode("测试");
            Assert.IsNotNull(ttn);
            Assert.AreEqual(POSType.D_N, (POSType)ttn.POSValue);
        }
        [Ignore]
        [Test]
        public void TestLoadTrieTreeFromMongoDB()
        {
            var mockProviderSetting = new Mock<IDataProviderSetting>();
            mockProviderSetting.Setup(s => s.Uri).Returns("mongodb://localhost:28001");
            mockProviderSetting.Setup(s => s.ProviderType).Returns("MongoDBDataProvider");
            mockProviderSetting.Setup(s => s.DBName).Returns("nameResearch");
            mockProviderSetting.Setup(s => s.CollectionName).Returns("placeNames");
            var dataProvider = new MongoDBDataProvider(mockProviderSetting.Object);
            TrieTree tt = TrieFactory.LoadFromDataProvider(dataProvider);
            Assert.IsNotNull(tt);
            Assert.IsNotNull(tt.Root);
            Assert.IsTrue(tt.Root.Children.Count > 100);
        }
    }
}
