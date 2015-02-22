using System;
using System.Collections.Generic;
using System.Text;

namespace Nepy.Core
{
    public class NumeralUtil
    {
        // 一般大写中文数字组
        static char[] chnFullCharText = new char[] {'０', '１', '２', '３', '４', '５', '６', '７', '８', '９' };

        static char[] chnGenText = new char[] { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九', '两' };
        static char[] chnGenDigit = new char[] { '十', '百', '千', '万', '亿', '兆', '卅', '廿' };
        static char[] koreaNumText = { '영', '일', '이', '삼', '사', '오', '육', '칠', '팔', '구' };
        static char[] koreaDigit = { '십', '백', '천', '만', '억' };

        static char[] jpnNumText = new char[] { '〇', '壹', '貳', '叁', '肆', '伍', '陸', '柒', '捌', '玖' };
        static char[] jpnDigit = new char[] { '什', '佰', '仟', '萬', '億' };
 
        // 人民币专用数字组
        static char[] chnRMBText = new char[] { '零', '壹', '贰', '叁', '肆', '伍', '陆', '染', '捌', '玖' };
        static char[] chnRMBDigit = new char[] { '拾', '佰', '仟', '萬', '億' };

        static char[] chnAllText = new char[] { '零', '壹', '贰', '叁', '肆', '伍', '陆', '染', '捌', '玖', '十', 
                '百', '千', '万', '亿', '一', '二', '三', '四', '五', '六', '七','八', '九', 
                '拾', '佰', '仟', '萬', '億','０', '１', '２', '３', '４', '５', '６', '７', '８', '９','兆', '卅','廿','两' };

        public static bool IsCJKNumber(char ch)
        {
            for (int i = 0; i < chnGenText.Length; i++)
            {
                if (ch == chnGenText[i])
                    return true;
            }

            if (NumeralUtil.IsArabicNumeral(ch))
            {
                return true;
            }

            for (int i = 0; i < chnRMBText.Length; i++)
            {
                if (ch == chnRMBText[i])
                    return true;
            }
            return false;
        }
        /// <summary>
        /// if the text is one,two,three,four,five,six,steven,eight,nine,ten
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsEnglishNumeral(string text)
        {
            if (text.Length < 3||text.Length>8)
                return false;

            char[] chars=text.ToLower().ToCharArray();
            char ch0 = chars[0];

            if (ch0 != 'o' && ch0 != 't' && ch0 != 'f'
                && ch0 != 's' && ch0 != 'e' && ch0 != 'n' && ch0 != 'b' && ch0 != 'm')
                return false;

            if (text.Length == 3 && (text == "one" || text == "two" || text == "ten" || text == "six"))
            {
                return true;
            }
            else if (text.Length == 4 && (text == "four" || text == "five" || text == "nine"))
            {
                return true;
            }
            else if (text.Length == 5 && (text == "three" || text == "eight"))
            {
                return true;
            }
            else if (text.Length == 6 && text == "steven" )
            {
                return true;
            }
            else if (text.Length == 7 && (text == "billion" || text == "million"))
            {
                return true;
            }
            else if (text.Length == 8 && text == "thousand" )
            {
                return true;
            }
            return false;
        }
        public static bool IsArabicNumeral(char ch)
        {
            if (ch >= '0' && ch <= '9')
            {
                return true;
            }
            return false;
        }
        public static bool IsChineseNumeralChars(char ch)
        {
            if (NumeralUtil.IsArabicNumeral(ch))
            {
                return false;
            }
            if (CharacterUtil.IsLantingLetter(ch))
            {
                return false;
            }

            for (int j = 0; j < chnAllText.Length; j++)
            {
                if (ch == chnAllText[j])
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsChineseGenDigit(char ch)
        {
            for (int j = 0; j < chnGenDigit.Length; j++)
            {
                if (ch == chnGenDigit[j])
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsChineseNumeral(char ch)
        {
            if (NumeralUtil.IsArabicNumeral(ch))
            {
                return false;
            }
            if (CharacterUtil.IsLantingLetter(ch))
            {
                return false;
            }
            for (int j = 0; j < chnGenText.Length; j++)
            {
                if (ch == chnGenText[j] || ch == chnRMBText[j] || ch == chnFullCharText[j])
                {
                    return true;
                }
            }
            return false;
        }
        private static string PhraseChineseNumeral(StringBuilder text)
        {
            double sum = 0;
            double t=0;
            double x = 1;
            bool isPrevNumeral = false;

            for (int j = 0; j < text.Length; j++)
            {
                char ch = text[j];
                if (ch == '负')
                {
                    x = x * -1;
                    continue;
                }
                
                bool handled = false;
                for (int k = 0; k < chnGenText.Length; k++)
                {
                    if (ch == chnGenText[k] || (k<chnRMBText.Length&&ch == chnRMBText[k]))
                    {

                        if (isPrevNumeral)
                            t = t * 10 + k;
                        else
                            t = k;
                        handled = true;
                        isPrevNumeral = true;
                        break;
                    }
                }
                if (handled)
                    continue;
                for (int k = 0; k < chnGenDigit.Length; k++)
                {
                    if (ch == chnGenDigit[k] || ch == chnRMBDigit[k])
                    {
                       sum = sum + t * Math.Pow(10, k + 1);
                       t = 0;
                       isPrevNumeral = false;
                       break;
                    }
                }
            }
            sum = sum + t;
            return (sum*x).ToString();
        }
        public static int ConvertCJKToArabic(char ch)
        {
            switch (ch)
            {
                case '0':
                case '零':
                case '〇':   
                case '영':
                    return '0';
                case '1':
                case '一':
                case '壹':
                case '일':
                    return '1';
                case '2':
                case '二':
                case '两':
                case '俩':
                case '貳':
                case '이':
                    return '2';
                case '3':
                case '三':
                case '叁':
                case '삼':
                    return '3';
                case '4':
                case '四':
                case '肆':
                case '사':
                    return '4';
                case '5':
                case '五':
                case '伍':
                case '오':
                    return '5';
                case '6':
                case '六':
                case '陸':
                case '육':
                    return '6';
                case '7':
                case '柒':
                case '七':
                case '칠':
                    return '7';
                case '8':
                case '捌':
                case '八':
                case '팔':
                    return '8';
                case '9':
                case '九':
                case '玖':   
                case '구':
                    return '9';
                case '十':
                case '什':   
                case '십':
                    return 10;
                case '百':
                case '佰':
                case '백':   
                    return 100;
                case '千':
                case '仟':
                case '천':
                    return 1000;
                case '万':
                case '萬':
                case '만':
                    return 10000;
                case '亿':
                case '億':
                case '억':
                    return 100000000;
                //case '兆':
                //case '조':
                //    return 1000000000000;
                default:
                    return -1;
            }
        }
        public static string ConvertChineseNumeral2Arabic(string text)
        {
            StringBuilder sb = new StringBuilder();
            char[] chars=text.ToCharArray();

            for (int i = 0; i < chars.Length; )
            {
                char ch = chars[i];

                StringBuilder sbChinesePhrase = new StringBuilder();
                if (ch == '负'&&
                    i+1<chars.Length&&IsChineseNumeralChars(chars[i+1]))
                {
                    sbChinesePhrase.Append(ch);
                    ch=chars[++i];
                }
                while (IsChineseNumeralChars(ch))
                {
                    sbChinesePhrase.Append(ch);
                    if (i + 1 < chars.Length)
                        ch = chars[++i];
                    else
                        ch='\0';
                }
                if (sbChinesePhrase.Length != 0)
                {
                    string tmp = PhraseChineseNumeral(sbChinesePhrase);
                    sb.Append(tmp);
                }
                if(ch!='\0')
                    sb.Append(ch);
                i++;
            }
            return sb.ToString();
        }
    }
}
