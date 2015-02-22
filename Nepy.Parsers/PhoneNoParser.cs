using Nepy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Parsers
{
    /// <summary>
    /// Phone Number Parser for different countries
    /// </summary>
    [ParserOrder(30)]
    public class PhoneNoParser : IParser
    {

        static readonly int[] countryCodes = { 244,93,355,213,376,1264,1268,54,374,247,61,43,994,1242,973,880,1246,375,32,501,229,1441,591,267,55,673,359,226,95,257,237,1,1345,236,235,
                                                 56,86,57,242,682,506,53,357,420,45,253,1890,593,20,503,372,251,679,358,33,594,241,220,995,49,233,350,30,1809,1671,502,224,592,509,504,
                                                 852,36,354,91,62,98,964,353,972,39,225,1876,81,962,855,327,254,82,965,331,856,371,961,266,231,218,423,370,352,853,261,265,60,960,223,
                                                 356,1670,596,230,52,373,377,976,1664,212,258,264,674,977,599,31,64,505,227,234,850,47,968,92,507,675,595,51,63,48,689,351,1787,974,262,
                                                 40,7,1758,1784,684,685,378,239,966,221,248,232,65,421,386,677,252,27,34,94,1758,1784,249,597,268,46,41,963,886,992,255,66,228,676,1809,
                                                 216,90,993,256,380,971,44,1,598,233,58,84,967,381,263,243,260
                                             };
        ParserContext context;
        public PhoneNoParser(ParserContext context)
        {
            this.context = context;
        }
        public ParserContext Context
        {
            get { return this.context; }
        }
        public static bool IsValidCountryCodes(string code)
        {
            if(code==null)
                return false;
            if(code.Length>3)
                return false;
            int iCode = Int32.Parse(code);
            for (int i = 0; i < countryCodes.Length; i++)
            {
                if (countryCodes[i] == iCode)
                    return true;
            }
            return false;
        }
        bool IsAllowedChar(char ch, ParserPattern pattern)
        {
            if (pattern == ParserPattern.China)
            {
                if (ch >= '０' && ch <= '９')
                    return true;
                if (NumeralUtil.IsArabicNumeral(ch))
                    return true;
                if (ch == '-' || ch == '－' || ch == '—')
                    return true;
                if (ch == '（' || ch == '(' || ch == ')' || ch == '）')
                    return true;
                if (ch == '+')
                    return true;
                if (ch == '#')
                    return true;
                if (ch == ' ' || ch == '　')
                    return true;
            }
            else if (pattern == ParserPattern.NorthAmerica)
            {
                if (NumeralUtil.IsArabicNumeral(ch))
                    return true;
                if (ch == '-' || ch == '－')
                    return true;
                if (ch == '(' || ch == ')')
                    return true;
                if (ch == '+')
                    return true;
                if (ch == ' ')
                    return true;
                if (ch == 'e' || ch == 'x' || ch == 't' || ch == '.')
                    return true;
            }
            return false;
        }

        /*
         中国移动：
        2G号段：134、135、136、137、138、139、150、151、152、157、158、159；
        3G号段：187、188；182(TD-SCDMA)

        中国联通：
        2G号段：130、131、132、155、156；
        3G号段：185、186；

        中国电信：
        2G号段：133、153；
        3G号段：180、189
         */
        internal static bool IsChineseMobilePrefix(char[] chars, ParserPattern pattern)
        {
            if (chars.Length < 3)
                throw new ArgumentException("To determine mobile prefix, the text length must be longer than 3");
            if (pattern == ParserPattern.China)
            {
                if (chars[0] != '1')
                    return false;
                if (chars[1] != '3' && chars[1] != '8' && chars[1] != '5')     //前缀必须是13, 15, 18
                    return false;
                if (chars[1] == '5' && chars[2] == '4') //联通或电信 除154外
                    return false;
                if (chars[1] == '8' && (chars[2] == '1' || chars[2] == '3' || chars[2] == '4'))   //网通前缀: 189, 188, 180, 185, 186, 187
                    return false;
            } if (pattern == ParserPattern.NorthAmerica)
            {
                return true;
            }
            return true;
        }
        public static bool IsMobileNo(string text, ParserPattern pattern)
        {
            char[] chars = text.Trim().ToCharArray();
            if (pattern == ParserPattern.China)
            {
                if (chars.Length != 11)
                    return false;
            }
            return IsChineseMobilePrefix(chars, pattern);
        }

        void AssignPhoneMain(StringBuilder segment, PhoneNo phone)
        {
            ParserPattern _pattern = context.Pattern;
            char separator = ' ';
            int bSeparatorLen = 0;

            if (_pattern == ParserPattern.NorthAmerica)
            {
                separator = '-';
            }
            if (phone.Main == null)
            {
                phone.Main = segment.ToString();
            }
            else if (phone.Main.Length < 5)
            {
                phone.Main += separator + segment.ToString();
                bSeparatorLen = 1;
            }
            if (IsMobileNo(phone.Main, _pattern))
            {
                phone.IsMobile = true;
            }
            else
            { 
                //非手机号码，8位
                if (_pattern == ParserPattern.China && phone.Main.Length > 8 + bSeparatorLen)
                {
                    phone.Main = null;
                }
            }

        }
        #region IParser Members

        public ParseResultCollection Parse(int startIndex)
        {
            string _text = context.Text;
            ParserPattern _pattern = context.Pattern;
            int k = startIndex;
            char ch;
            StringBuilder sb = new StringBuilder(10);
            ParseResultCollection prc = new ParseResultCollection();
            if (_text[startIndex] == ' ' || _text[startIndex] == '　')
                return prc;

            int braceStartPos = -1;
            while (k < _text.Length)
            {
                ch = _text[k];
                if (!IsAllowedChar(ch,_pattern))
                    break;
                if (ch >= '０' && ch <= '９')
                    ch = (char)(ch - '０' + '0');
                if (ch == '　')
                {
                    ch = ' ';
                }
                else if (ch == '（')
                    ch = '(';
                else if (ch == '）')
                    ch = ')';
                else if (ch == '－' || ch == '—')
                    ch = '-';

                if (ch == '(')
                    braceStartPos = k;
                else if (ch == ')')
                    braceStartPos = -1;
                sb.Append(ch);
                k++;
            }
            string allowedString = sb.ToString().TrimEnd();
            if (braceStartPos >= 0)
            {
                allowedString = allowedString.Substring(0, braceStartPos);
            }
            
            if (allowedString.Length<3||allowedString.Length==4)
                return prc;

            bool bNumberInBrace = false;
            bool bCountryCodeStarted = false;
            bool bAreaCodeStarted = false;
            bool bExtStarted = false;
            int i = 0;

            StringBuilder segment = new StringBuilder();
            StringBuilder whole = new StringBuilder();

            PhoneNo phone = new PhoneNo();

            if (_pattern == ParserPattern.China)
            {

                while (i < allowedString.Length)
                {
                    ch = allowedString[i];
                    if (ch == '(')
                    {
                        bNumberInBrace = true;
                        bCountryCodeStarted = false;
                        whole.Append(ch);
                    }
                    else if (NumeralUtil.IsArabicNumeral(ch))
                    {
                        if (segment.Length == 0 && !bAreaCodeStarted
                            && phone.AreaCode == null && !bCountryCodeStarted)
                            bAreaCodeStarted = true;

                        segment.Append(ch);
                        whole.Append(ch);
                    }
                    else if (ch == ')' && bNumberInBrace)
                    {
                        if (bCountryCodeStarted)
                        {
                            if (segment.Length > 0)
                                phone.CountryCode = segment.ToString();
                            bCountryCodeStarted = false;
                        }
                        if (bAreaCodeStarted)
                        {
                            if (segment.Length > 0 && (segment[0] == '0' ? segment.Length <= 4 : segment.Length <= 3))  //城市代码以0开头，最多4个数字；不以0开头，三个数字
                                phone.AreaCode = segment.ToString();
                            bAreaCodeStarted = false;
                        }
                        whole.Append(ch);
                        segment = new StringBuilder();
                        bNumberInBrace = false;
                    }
                    else if (ch == ' ')
                    {
                        if (bCountryCodeStarted)
                        {
                            if (segment.Length > 0)
                                phone.CountryCode = segment.ToString();
                            bCountryCodeStarted = false;
                        }
                        else if (bAreaCodeStarted)
                        {
                            if (segment.Length > 0)
                                phone.AreaCode = segment.ToString();
                            bAreaCodeStarted = false;
                        }
                        else if (segment.Length > 0)
                        {
                            AssignPhoneMain(segment, phone);
                        }
                        segment = new StringBuilder();
                        bCountryCodeStarted = false;
                        whole.Append(ch);
                    }
                    else if (ch == '-' || ch == '#')
                    {
                        if (segment[0] == '0' && (segment.Length == 3 || segment.Length == 4))
                        {
                            phone.AreaCode = segment.ToString();
                        }else if (segment.Length > 0)
                        {
                            AssignPhoneMain(segment, phone);
                            bExtStarted = true;
                        }                        
                        segment = new StringBuilder();
                        whole.Append(ch);
                    }
                    else if (ch == '+')
                    {
                        whole.Append(ch);
                        bCountryCodeStarted = true;
                    }
                    i++;
                }
                if (segment.Length > 0)
                {
                    AssignPhoneMain(segment, phone);
                    if (bExtStarted)
                    {
                        phone.Extension = segment.ToString();
                        bExtStarted = false;
                    }
                }
            }
            else if (_pattern == ParserPattern.NorthAmerica)
            {
                while (i < allowedString.Length)
                {
                    ch = allowedString[i];

                    if (NumeralUtil.IsArabicNumeral(ch))
                    {
                        whole.Append(ch);
                        segment.Append(ch);
                    }
                    else if (ch == ' ')
                    {
                        whole.Append(ch);
                    }
                    else if (ch == '(')
                    {
                        bAreaCodeStarted = true;
                        whole.Append(ch);
                    }
                    else if (ch == ')')
                    {
                        if (bAreaCodeStarted)
                        {
                            if (segment.Length > 0)
                                phone.AreaCode = segment.ToString();
                            bAreaCodeStarted = false;
                        }
                        segment = new StringBuilder();
                        whole.Append(ch);
                    }
                    else if (ch == '-')
                    {
                        if (bCountryCodeStarted)
                        {
                            if (segment.Length > 0)
                                phone.CountryCode = segment.ToString();
                            bCountryCodeStarted = false;
                            bAreaCodeStarted = true;
                        }
                        else if (bAreaCodeStarted)
                        {
                            if (segment.Length > 0)
                                phone.AreaCode = segment.ToString();
                            bAreaCodeStarted = false;
                        }
                        else if (segment.Length > 0)
                        {
                            AssignPhoneMain(segment, phone);
                        }
                        whole.Append(ch);
                        segment = new StringBuilder();
                    }
                    else if (ch == '+')
                    {
                        bCountryCodeStarted = true;
                        whole.Append(ch);
                    }
                    else if (ch == '.')
                    {
                        if (segment.ToString() != "ext")
                            break;

                        whole.Append("ext.");
                    }
                    else if (ch == 'e' || ch == 'x' || ch == 't')
                    {
                        segment.Append(ch);
                    }
                    i++;
                }
                if (segment.Length > 0)
                {
                    AssignPhoneMain(segment, phone);
                    if (bExtStarted)
                    {
                        phone.Extension = segment.ToString();
                        bExtStarted = false;
                    }
                }
            }
            else
            {
                throw new NotImplementedException("Phone No. in "+_pattern.ToString()+" is not implemented in the parser.");
            }
            if (whole.Length > 0 && phone.Main!=null)
            {
                prc.Add(ParseResult.Create(whole.ToString(), startIndex, POSType.A_M, phone));
            }
            return prc;
        }

        #endregion
    }
}
