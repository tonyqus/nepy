using Nepy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Parsers
{
    /// <summary>
    /// 代词解析器（包括人称代词，指示代词，疑问代词）
    /// </summary>
    /// <remarks>
    /// http://xh.5156edu.com/page/z2190m2907j18579.html
    /// </remarks>
    [ParserDefaultOrder(3)]
    public class SimChinesePronounParser:IParser
    {
        ParserContext context;
        public SimChinesePronounParser(ParserContext context)
        {
            this.context = context;
        }

        #region IParser Members

        public ParserContext Context
        {
            get { return this.context; }
        }

        public ParseResultCollection Parse(int startIndex)
        {
            char[] chars = context.Text.ToArray();
            ParseResultCollection prc = new ParseResultCollection();
            int i=startIndex;
            StringBuilder sb = new StringBuilder();

            if (chars[i] == '这'||chars[i] == '那')
            {
                sb = new StringBuilder();
                sb.Append(chars[i]);
                if (i + 1 < context.Text.Length)
                {
                    char nextchar = chars[i + 1];
                    if (nextchar == '些' || nextchar == '里'|| nextchar=='儿')
                        sb.Append(nextchar);
                }   
            }
            else if (chars[i] == '你'||chars[i] == '我'
                || chars[i] == '他' || chars[i] == '它' || chars[i] == '她'||chars[i]=='咱')
            {
                sb = new StringBuilder();
                sb.Append(chars[i]);
                if (i + 1 < context.Text.Length)
                {
                    char nextchar = chars[i + 1];
                    if (nextchar == '们')
                        sb.Append(nextchar);
                }
            }
            else if (chars[i] == '谁' || chars[i] == '朕' || chars[i] == '此' || chars[i] == '彼')
            {
                sb = new StringBuilder();
                sb.Append(chars[i]);
            }
            else if (chars[i] == '大')
            {
                sb = new StringBuilder();
                if (i + 1 < context.Text.Length && chars[i + 1] == '家')
                    sb.Append("大家");
            }
            else if (chars[i] == '什')
            {
                sb = new StringBuilder();
                if (i + 1 < context.Text.Length && chars[i + 1] == '么')
                    sb.Append("什么");
            }
            else if (chars[i] == '自')
            {
                sb = new StringBuilder();
                if (i + 1 < context.Text.Length && chars[i + 1] == '己')
                    sb.Append("自己");
            }
            else if (chars[i] == '哪')
            {
                sb = new StringBuilder();
                sb.Append(chars[i]);
                if (i + 1 < context.Text.Length)
                {
                    char nextchar = chars[i + 1];
                    if (nextchar == '里')
                        sb.Append(nextchar);
                }
            }
            if (sb.Length > 0)
            {
                ParseResult pr = new ParseResult();
                pr.StartPos = i;
                pr.Text = sb.ToString();
                pr.Type = POSType.D_R;
                prc.Add(pr);
                sb = new StringBuilder();
            }

            return prc;
        }

        #endregion
    }
}
