using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Core
{
    public static class StringHelpers
    {
        public static string GetWord(this string sentance, int wordCount)
        {
            return sentance.Split()[wordCount - 1];
        }

        public static string GetStringAfterWord(this string sentance, int wordCount)
        {
            string returnString = String.Empty;
            string[] words = sentance.Split();
            for (int i = wordCount; i < words.Length; i++)
            {
                returnString += words[i] + " ";
            }
            return returnString.TrimEnd();
        }
    }
}
