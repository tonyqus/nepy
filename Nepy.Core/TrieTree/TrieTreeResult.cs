using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Core
{
    public class TrieTreeResult
    {
        public TrieTreeResult(string word):this(word,0,0)
        {
        }

        public TrieTreeResult(string word, int pos, int freq)
        {
            this.Word = word;
            this.POS = pos;
            this.Frequency = freq;
        }

        public string Word { get; set; }
        public int POS { get; set; }
        public int Frequency { get; set; }
    }
}
