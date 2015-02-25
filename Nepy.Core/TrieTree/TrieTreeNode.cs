using System;
using System.Collections.Generic;
using System.Linq;

namespace Nepy.Core
{
    public class TrieTreeNode
    {
        public TrieTreeNode(char ch,int depth)
        {
            this.Character=ch;
            this._depth=depth;
        }


        public char Character;

        int _depth;
        public int Depth
        {
            get{return _depth;}
        }


        TrieTreeNode _parent=null;
        public TrieTreeNode Parent 
        {
            get { return _parent; }
            set { _parent = value; }
        }

        int _posvalue=0;
        public int POSValue
        {
            get { return _posvalue; }
        }

        public void AddPOS(POSType pos)
        {
            _posvalue = _posvalue | (int)pos;
        }
        public void RemovePOS(POSType pos)
        {
            _posvalue = _posvalue & ~(int)pos;
        }

        public bool WordEnded = false;


        HashSet<TrieTreeNode> _children=null;
        public HashSet<TrieTreeNode> Children
        {
            get {
                if (_children == null)
                {
                    _children = new HashSet<TrieTreeNode>();
                }
                return _children; 
            }
        }

        public TrieTreeNode GetChildNode(char ch)
        {
            if (_children != null)
                return _children.FirstOrDefault(n => n.Character == ch);
            else
                return null;
        }

        public TrieTreeNode AddChild(char ch)
        {
            TrieTreeNode matchedNode=null;
            if (_children != null)
            {
                matchedNode = _children.FirstOrDefault(n => n.Character == ch);
            }
            if (matchedNode != null)    //found the char in the list
            {
                //matchedNode.IncreaseFrequency();
                return matchedNode;
            }
            else
            {   //not found
                TrieTreeNode node = new TrieTreeNode(ch, this.Depth + 1);
                node.Parent = this;
                //node.IncreaseFrequency();
                if (_children == null)
                    _children = new HashSet<TrieTreeNode>();
                _children.Add(node);
                return node;
            }
        }

        double _frequency = 0;
        public double Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        public void IncreaseFrequency()
        {
            _frequency++;
        }

        public string Word
        {
            get
            {
                TrieTreeNode tmp = this;
                string result = string.Empty;
                while (tmp.Parent != null) //until root node
                {
                    result = tmp.Character + result;
                    tmp = tmp.Parent;
                }
                return result;
            }
        }

        public override string ToString()
        {
            return Convert.ToString(this.Character);
        }
    }
}
