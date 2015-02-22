using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Core
{
    public class ParseResultCollection : List<ParseResult>
    {

        public ParseResultCollection()
        {
        }
        internal static ParseResultCollection InternalParse(string text, IParser parser)
        {
            int i = 0;
            ParseResultCollection prc = new ParseResultCollection();

            while (i < text.Length)
            {
                ParseResultCollection prcTemp = parser.Parse(i);
                if (prcTemp.Count > 0)
                {
                    foreach (ParseResult pr in prcTemp)
                    {
                        prc.Add(pr);
                        i += pr.Length;
                    }
                }
                else
                {
                    i++;
                }
            }
            return prc;
        }
    }

    public class ParseResult
    {

        public int StartPos;

        /// <summary>
        /// length of word text
        /// </summary>
        public int Length
        {
            get { return Text.Length; }
        }
        public POSType Type { get; set; }
        public string Text { get; set; }
        public object Value { get; set; }

        /// <summary>
        /// create a instance of ParseResult
        /// </summary>
        /// <param name="text">word text</param>
        /// <param name="startIndex">start position</param>
        /// <param name="type">POS value</param>
        /// <returns></returns>
        public static ParseResult Create(string text, int startIndex, POSType type)
        {
            return Create(text, startIndex, type, null);
        }
        /// <summary>
        /// create a instance of ParseResult
        /// </summary>
        /// <param name="text">word text</param>
        /// <param name="startIndex">start position</param>
        /// <param name="type">POS value</param>
        /// <param name="value">.NET value for this word</param>
        /// <returns></returns>
        public static ParseResult Create(string text, int startIndex, POSType type, object value)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex must be bigger than 0.");
            ParseResult pr = new ParseResult();
            pr.Text = text.Trim();
            pr.StartPos = startIndex;
            pr.Type = type;
            pr.Value = value;
            return pr;
        }
        /// <summary>
        /// get the string result
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", this.Text, this.StartPos, Type.ToString());
        }
    }
}
