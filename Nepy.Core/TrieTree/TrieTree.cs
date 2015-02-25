using Nepy.Dictionary;
using System.Collections.Generic;
using System.Linq;

namespace Nepy.Core
{
    public class TrieTree
    {
        TrieTreeNode _root = null;

        private TrieTree()
        {
            _root = new TrieTreeNode(char.MaxValue,0);
        }
        static TrieTree _instance = null;
        public static TrieTree GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TrieTree();
            }
            return _instance;
        }
        public TrieTreeNode Root 
        {
            get { return _root; }
        }
        public TrieTreeNode AddWord(char ch, POSType pos)
        {
            return AddWord(ch, double.NaN, true, pos);
        }
        public TrieTreeNode AddWord(char ch, double frequency, POSType pos)
        {
            return AddWord(ch, frequency, true, pos);
        }
        internal TrieTreeNode AddWord(char ch, double frequency, bool isWordEnded, POSType pos)
        {
            TrieTreeNode newnode = _root.AddChild(ch);
            if (frequency == double.NaN)
                newnode.IncreaseFrequency();
            else
                newnode.Frequency += frequency;
            newnode.WordEnded = true;
            if (pos != POSType.UNKNOWN && pos != POSType.NEWLINE)
                newnode.AddPOS(pos);
            return newnode;
        }

        public TrieTreeNode AddWord(string word)
        {
            return AddWord(word, double.NaN);
        }
        public TrieTreeNode AddWord(string word, POSType pos)
        {
            return AddWord(word, double.NaN, pos);
        }
        public TrieTreeNode AddWord(string word, double frequency, POSType pos)
        {
            if (word.Length == 1)
            {
                return AddWord(word[0], frequency, pos);
            }
            else
            {
                char[] chars = word.ToCharArray();
                TrieTreeNode node = _root;
                for (int i = 0; i < chars.Length; i++)
                {
                    TrieTreeNode newnode = node.AddChild(chars[i]);
                    node = newnode;
                }
                if (frequency == double.NaN)
                    node.IncreaseFrequency();
                else
                    node.Frequency += frequency;
                node.WordEnded = true;
                if (pos != POSType.UNKNOWN && pos != POSType.NEWLINE)
                    node.AddPOS(pos);
                return node;
            }
        }
        public TrieTreeNode AddWord(string word, double frequency)
        {
            return AddWord(word, frequency, POSType.UNKNOWN);
        }

        public List<string> GetTopFrequencyWords(int n,int topnum)
        {
            Dictionary<string,double> list = GetNGramList(n);
            var sortedDict = (from entry in list
                              orderby entry.Value descending
                              select entry).Take(topnum).ToDictionary(p => p.Key,p=>p.Value);
            return sortedDict.Keys.ToList();
        }
        public TrieTreeNode GetNode(char ch)
        {
            return GetNode(ch, 0);
        }
        public TrieTreeNode GetNode(char ch, int pos)
        {
            TrieTreeNode matchedNode = null;
            if (pos==0)
                matchedNode = _root.Children.FirstOrDefault(n => n.Character == ch);
            else
                matchedNode = _root.Children.FirstOrDefault(n => n.Character == ch && (n.POSValue & pos)>0);
            if (matchedNode == null)
            {
                return null;
            }
            return matchedNode;
        }
        public TrieTreeNode GetNode(string word)
        {
            return GetNode(word, 0);
        }
        public TrieTreeNode GetNode(string word, int pos)
        {
            if (word.Length == 1)
            {
                return GetNode(word[0], pos); 
            }
            else
            {
                char[] chars = word.ToCharArray();
                TrieTreeNode node = _root;
                for (int i = 0; i < chars.Length; i++)
                {
                    if (node.Children == null)
                        return null;
                    TrieTreeNode matchednode = node.Children.FirstOrDefault(n => n.Character == chars[i]);
                    if (matchednode == null)
                    {
                        return null;
                    }
                    node = matchednode;
                }
                if (node.WordEnded == true)
                {
                    if (pos == 0)
                        return node;
                    else if ((node.POSValue & pos) >0)
                        return node;
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        void NGramRecursive(Dictionary<string,double> dict,TrieTreeNode tn,string r,int depth)
        {
            string nr = r + tn.Character;
            if (nr != string.Empty&&tn.Depth==depth)
            {
                dict.Add(nr, tn.Frequency);
            }
            if (tn.Children== null||tn.Depth==depth)
            {
                return;
            }
            else
            {
                foreach(TrieTreeNode tnc in tn.Children)
                {
                    NGramRecursive(dict, tnc,nr,depth);
                }
            }
            
        }

        public Dictionary<string,double> GetNGramList(int n)
        {
            var dict = new Dictionary<string, double>(100);
            NGramRecursive(dict, _root, string.Empty, n);
            return dict;
        }
        
        public void Load(IDataProvider provider)
        {
            if (provider == null)
                return;
            var list = provider.Load();
            foreach (IDataNode wd in list)
            {
                if (wd.Word == null)
                    continue;
                this.AddWord(wd.Word,wd.Frequency,wd.POS);
            }
        }

        const int FrequencyThreshold=3;
    }
}
