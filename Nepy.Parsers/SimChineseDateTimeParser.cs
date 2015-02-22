using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Nepy.Core;

namespace Nepy.Parsers
{
    /// <summary>
    /// DateTime parser for simplified chinese text
    /// </summary>
    [ParserOrder(35)]
    public class SimChineseDateTimeParser:IParser
    {
        ParserContext context;
        const int maxDateTimeTextLength = 15;

        public SimChineseDateTimeParser(ParserContext context)
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
            string text = NumeralUtil.ConvertChineseNumeral2Arabic(context.Text);
            ParseResultCollection prc = new ParseResultCollection();

            int boundary = Math.Min(maxDateTimeTextLength, text.Length - startIndex);
            string temp = text.Substring(startIndex, boundary);
            StringBuilder sbDateText = new StringBuilder();
            StringBuilder sbPatternText = new StringBuilder();
            StringBuilder sbText = new StringBuilder();
            int strLen = 0;
            int i;
            char prevCh=' ';
            bool nonNumeric = false;
            for (i = 0; i < boundary; i++)
            {
                char ch = temp[i];
                
                if (NumeralUtil.IsArabicNumeral(ch))
                {
                    sbDateText.Append(ch);
                    sbText.Append(ch);
                    strLen++;
                }
                else if (ch == '大' || ch == '前' || ch == '昨' || ch == '明' || ch == '今' || ch == '后'|| ch == '去')
                {

                }
                else if (ch == '周')
                {
                    if (prevCh == '上')
                    {
                        nonNumeric = true;
                        sbText.Append(prevCh);
                        sbText.Append(ch);
                        break;
                    }
                }
                else if (ch == '天')
                {
                    if (prevCh == '前' || prevCh == '昨' || prevCh == '明' || prevCh == '今' || prevCh == '后')
                    {
                        nonNumeric = true;
                        sbText.Append(prevCh);
                        sbText.Append(ch);
                        break;
                    }
                }
                else if (ch == '年')
                {
                    if (prevCh == '去' || prevCh == '前' || prevCh == '今' || prevCh == '后')
                    {
                        nonNumeric = true;
                        sbText.Append(prevCh);
                        sbText.Append(ch);
                        break;
                    }
                    if (strLen == 0) 
                        return prc;
                    sbDateText.Append(ch);
                    sbPatternText.Append(DateUtil.GeneratePatternText('y', strLen));
                    sbPatternText.Append(ch);
                    strLen = 0;
                    sbText.Append(ch);
                }
                else if (ch == '日')
                {
                    if (strLen == 0)    
                        return prc;

                    sbDateText.Append(ch);
                    sbPatternText.Append(DateUtil.GeneratePatternText('d', strLen));
                    sbPatternText.Append(ch);
                    strLen = 0;
                    sbText.Append(ch);
                }
                else if (ch == '月')
                {
                    if (strLen == 0) 
                        return prc;

                    sbDateText.Append(ch);
                    sbPatternText.Append(DateUtil.GeneratePatternText('M', strLen));
                    sbPatternText.Append(ch);
                    sbText.Append(ch);
                    strLen = 0;
                }
                else if (ch == '分')
                {
                    if (strLen == 0) 
                        return prc;

                    sbDateText.Append(ch);
                    sbPatternText.Append(DateUtil.GeneratePatternText('m', strLen));
                    sbPatternText.Append(ch);
                    sbText.Append(ch);
                    strLen = 0;
                }
                else if (ch == '秒')
                {
                    if (strLen == 0) 
                        return prc;

                    sbDateText.Append(ch);
                    sbPatternText.Append(DateUtil.GeneratePatternText('s', strLen));
                    sbPatternText.Append(ch);
                    sbText.Append(ch);
                    strLen = 0;
                }
                else if (ch == '点')
                {
                    if (strLen == 0) 
                        return prc;

                    sbDateText.Append(ch);
                    sbPatternText.Append(DateUtil.GeneratePatternText('h', strLen));
                    sbPatternText.Append(ch);
                    sbText.Append(ch);
                    strLen = 0;
                }
                else if (ch == ' ')
                {
                    sbText.Append(ch);
                    continue;
                }
                else
                {
                    break;
                }
                prevCh = ch;
            }
            if (sbText.Length >0 &&nonNumeric== true)
            {
                prc.Add(ParseResult.Create(sbText.ToString(),startIndex,POSType.D_T));
                return prc;
            }
            if (sbDateText.Length == 0 || sbPatternText.Length == 0)
            {
                return prc;
            }
            DateTime? dt = DateUtil.ParseDate(sbDateText.ToString(), sbPatternText.ToString());
            if (dt != null)
            {
                string result=sbText.ToString();
                prc.Add(ParseResult.Create(result,startIndex,POSType.D_T,dt));
            }
            return prc;
        }

        #endregion
    }
}
