using System;
using System.Collections.Generic;

using System.Text;

namespace Nepy.Core
{
    public class NGram
    {
        int _n;
        public int N
        {
            get
            { return _n; }
        }

        public NGram(int n)
        {
            _n = n;
        }

        public List<string> Analyze(string text)
        {
            List<string> list = new List<string>();
            char[] chars= text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)   //i < chars.Length - _n+1
            {
                string x = string.Empty;
                int k = (chars.Length - i)>_n?_n:(chars.Length - i);

                for (int j = 0; j < k; j++)
                {
                    if (CharacterUtil.IsCjkUnifiedIdeographs(chars[i + j]))
                        x += chars[i + j];
                    else
                        break;  //if the current char is puncuation, this ngram is stopped
                }
                if(!String.IsNullOrEmpty(x))
                    list.Add(x);
            }
            return list;
        }
    }
}
