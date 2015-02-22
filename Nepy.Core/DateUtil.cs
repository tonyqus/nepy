using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Nepy.Core
{
    public class DateUtil
    {
        public static string GeneratePatternText(char patternChar, int length)
        {
            return new string(patternChar, length);
        }

        public static DateTime? ParseDate(string text, string pattern)
        {
            DateTime dt = new DateTime();
            DateTime.TryParseExact(text, pattern, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dt);
            if (dt == DateTime.MinValue)
                return null;
            return dt;
        }

        public static string ConvertChineseText2DateText(string text,string formatPattern)
        {
            string errText = "err";

            text = NumeralUtil.ConvertChineseNumeral2Arabic(text);
            char[] chars = text.ToCharArray();
            StringBuilder sbDateText = new StringBuilder();
            StringBuilder sbPatternText = new StringBuilder();
            StringBuilder sbText = new StringBuilder();
            int strLen = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];

                if (NumeralUtil.IsArabicNumeral(ch))
                {
                    sbDateText.Append(ch);
                    strLen++;
                }
                else if (ch == '年')
                {
                    sbDateText.Append(ch);
                    sbPatternText.Append(GeneratePatternText('y', strLen));
                    sbPatternText.Append(ch);
                    strLen = 0;
                }
                else if ( ch == '日')
                {
                    sbDateText.Append(ch);
                    sbPatternText.Append(GeneratePatternText('d', strLen));
                    sbPatternText.Append(ch);
                    strLen = 0;
                }
                else if (ch == '月')
                {
                    sbDateText.Append(ch);
                    sbPatternText.Append(GeneratePatternText('M', strLen));
                    sbPatternText.Append(ch);
                    strLen = 0;
                }
                else if (ch == '分')
                {
                    sbDateText.Append(ch);
                    sbPatternText.Append(GeneratePatternText('m', strLen));
                    sbPatternText.Append(ch);
                    strLen = 0;
                }
                else if (ch == '秒')
                {
                    sbDateText.Append(ch);
                    sbPatternText.Append(GeneratePatternText('s', strLen));
                    sbPatternText.Append(ch);
                    strLen = 0;
                }
                else if (ch == '点')
                {
                    sbDateText.Append(ch);
                    sbPatternText.Append(GeneratePatternText('h', strLen));
                    sbPatternText.Append(ch);
                    strLen = 0;                    
                }
                else if (ch == '/')
                {
                    if (strLen==4)
                    {
                        sbPatternText.Append(GeneratePatternText('y', strLen));
                    }
                    else if (strLen==2)
                    {
                        sbPatternText.Append(GeneratePatternText('M', strLen));
                    }
                    sbPatternText.Append(ch);
                    sbDateText.Append(ch);
                    strLen = 0;
                }
                else if (ch == ' ')
                {
                    if (strLen == 2)
                    {
                        if (sbPatternText.ToString().IndexOf('M') >= 0)
                        {
                            sbPatternText.Append(GeneratePatternText('d', strLen));
                        }
                        else
                        {
                            sbPatternText.Append(GeneratePatternText('M', strLen));
                        }
                    }
                    continue;
                }
                else
                {
                    if (sbDateText.Length == 0)
                    {
                        sbText.Append(ch);
                        strLen = 0;
                        continue;
                    }
                    DateTime? dt = ParseDate(sbDateText.ToString(), sbPatternText.ToString());
                    if (dt != null)
                    {
                        sbText.AppendFormat(formatPattern==null?"[{0}]":"[{0:"+formatPattern+"}]", dt);
                    }
                    else
                    {
                        sbText.AppendFormat("[{0}]", errText);
                    }
                    //clear the string builder
                    sbDateText = new StringBuilder();
                    sbPatternText = new StringBuilder();
                    sbText.Append(ch);
                    strLen = 0;
                    continue;
                }
            }
            if (sbDateText.Length != 0)
            {
                DateTime? dt = ParseDate(sbDateText.ToString(), sbPatternText.ToString());
                if (dt != null)
                {
                    sbText.AppendFormat(formatPattern == null ? "[{0}]" : "[{0:" + formatPattern + "}]", dt);
                }
                else
                {
                    sbText.AppendFormat("[{0}]", errText);
                }   
            }
            return sbText.ToString();
        }
        public static int MonthDiff(DateTime date1, DateTime date2)
        {
            int monthSpan = (date2.Year - date1.Year) * 12 + date2.Month - date1.Month;
            return monthSpan;
        }
    }
}
