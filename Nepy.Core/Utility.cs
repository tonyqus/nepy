using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Nepy.Core
{
    public partial class Utility
    {
        /// <summary>
        /// Detect BOM header
        /// </summary>
        /// <param name="beginBytes">the first 2-3 bytes from a text file</param>
        /// <returns>Encoding Type</returns>
        public static Encoding DetectBOM(Stream file)
        {
            byte[] beginBytes = new byte[3];

            if (file.Length < 3)
            {
                throw new InvalidDataException("The length of file is less than 3 bytes");
            }
            beginBytes[0] = (byte)file.ReadByte();
            beginBytes[1] = (byte)file.ReadByte();
            beginBytes[2] = (byte)file.ReadByte();

            if (beginBytes[0] == 0xFF && beginBytes[1] == 0xFE)
            {
                file.Position = 2;
                return Encoding.Unicode;
            }
            
            if (beginBytes[0] == 0xFE && beginBytes[1] == 0xFF)
            {
                file.Position = 2;
                return Encoding.BigEndianUnicode;
            }
            
            if (beginBytes[0] == 0xEF && beginBytes[1] == 0xBB && beginBytes[2] == 0xBF)
            {
                file.Position = 3;
                return Encoding.UTF8;
            }
            
            file.Position = 0;
            return Encoding.ASCII;
        }

        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        public static bool CompareWithWhitespace(string str1, string str2)
        {
            int i=0,j = 0;
            while (i < str1.Length)
            {
                char ch1 = str1[i];
                char ch2 = str2[j];
                if (ch1 == ' '||ch1=='　')
                {
                    i++;
                    continue;
                }
                if (ch2 == ' ' || ch1 == '　')
                {
                    j++;
                    continue;
                }
                if (ch1 != ch2)
                {
                    return false;
                }
                i++;
                j++;
            }
            for (int k = j; k < str2.Length; k++)
            {
                if (str2[k] != ' ' && str2[k] != '　')
                    return false;
            }
            return true;
        }
        
        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
