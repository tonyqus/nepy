using Nepy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Parsers
{
    /// <summary>
    /// Postal code parser for different countries
    /// </summary>
    [ParserOrder(36)]
    public class PostalCodeParser:IParser
    {
        

        ParserContext context;
        public PostalCodeParser(ParserContext context)
        {
            this.context = context;
        }

        public ParserContext Context
        {
            get { return this.context; }
        }

        #region IParser Members

        public ParseResultCollection Parse(int startIndex)
        {
            string _text = context.Text;
            ParserPattern _format = context.Pattern;
            char ch;
            int i=startIndex;
            StringBuilder sb = new StringBuilder(6);
            ParseResultCollection prc = new ParseResultCollection();
            ch = _text[i];
            while (NumeralUtil.IsArabicNumeral(ch) || (ch >= '０' && ch <= '９') && i < _text.Length)
            {
                sb.Append(ch);
                ch = _text[++i];
            }
            string source = sb.ToString();

            if (_format == ParserPattern.China)
            { 
                if (source.Length !=6)
                    return prc;
            }
            else if (_format == ParserPattern.NorthAmerica)
            {
                if (source.Length != 5)
                    return prc;                                
            }
            prc.Add(ParseResult.Create(source.ToString(), startIndex, POSType.A_M));

            return prc;
        }

        #endregion
    }
}
