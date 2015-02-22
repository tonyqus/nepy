using System;
using System.Collections.Generic;

using System.Text;

namespace Nepy.Core
{
    public class CharacterUtil
    {
        public static bool IsSpaceLetter(char input)
        {
            return input == 8 || input == 9
                    || input == 10 || input == 13
                    || input == 32 || input == 160;
        }
        public static bool IsLantingLetter(char ch)
        {
            return ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z';
        }
        public static bool IsCjkUnifiedIdeographs(char ch)
        {
            return UnicodeBlock.Of(ch)==UnicodeBlock.CJK_UNIFIED_IDEOGRAPHS;
        }
        public static bool IsEmailCharacter(char ch)
        {
            return NumeralUtil.IsArabicNumeral(ch) || IsLantingLetter(ch) || ch == '_';
        }
        public static bool IsUrlCharacter(char ch)
        {
            return NumeralUtil.IsArabicNumeral(ch) || IsLantingLetter(ch) || ch == '_' || ch == '-';
        }
        public static bool IsUrlPrefix(string prefix)
        {
            return prefix == "ftp" || prefix == "http" || prefix == "https" || prefix == "news";
        }
        public static bool IsWesternPunctuation(char ch)
        {
            return ch == '\'' || ch == '\"' || ch == '?' || ch == '.' || ch == ':' || ch == ',' || ch == ';'
                || ch == '(' || ch == ')' || ch == '[' || ch == ']' || ch == '{' || ch == '}' || ch == '!'
                || ch == '/' || ch == '«' || ch == '»' || ch == '-' || ch == '—' || ch == '–' || ch == '"' || ch == '\'' || ch == '$';
        }
        public static bool IsChinesePunctuation(char ch)
        {
            return ch == '，' || ch == '。' || ch == '：' || ch == '？' || ch == '；' || ch == '“' || ch == '”' || ch == '‘'
                || ch == '’' || ch == '「' || ch == '」' || ch == '﹁' || ch == '﹂' || ch == '『' || ch == '』' || ch == '、'
                || ch == '·' || ch == '《' || ch == '》' || ch == '…' || ch == '—' || ch == '～' || ch == '（' || ch == '）';
        }
        public static bool IsJapanesePunctuation(char ch)
        {
            return ch == '。' || ch == '、' || ch == '：' || ch == '！' || ch == '？' || ch == '〜' || ch == '『' || ch == '』'
                || ch == '「' || ch == '」' || ch == '〽' || ch == '‥' || ch == '…' || ch == '（' || ch == '）' || ch == '｛'
                || ch == '｝' || ch == '［' || ch == '］' || ch == '【' || ch == '】';
        }
        /// <summary>
        /// 进行字符规格化（全角转半角，大写转小写处理）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static char Regularize(char input)
        {
            if (input == 12288)
            {
                input = (char)32;

            }
            else if (input > 65280 && input < 65375)
            {
                input = (char)(input - 65248);

            }
            else if (input >= 'A' && input <= 'Z')
            {
                input = (char)((int)input+32);
            }

            return input;
        }
    }
}
