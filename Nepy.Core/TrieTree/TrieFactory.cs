using Nepy.Dictionary;

namespace Nepy.Core
{
    public class TrieFactory
    {
        public static TrieTree LoadFromDataProvider(IDataProvider provider)
        {
            TrieTree tt = TrieTree.GetInstance();
            tt.Load(provider);
            return tt;
        }
    }
}
