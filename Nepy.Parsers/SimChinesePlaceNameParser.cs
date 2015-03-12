using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BluePrint.SegmentFramework;
using BluePrint.NLPCore;
using BluePrint.Dictionary;

namespace BluePrint.SegmentFramework.Parser
{
    [ParserOrder(40)]
    public class PlaceNameParser:IParser
    {
        public const int maxChinesePlaceNameLength = 8;
        public static DictionaryServiceClient placeNamesTrie = null;

        ParserContext context;
        public ParserContext Context
        {
            get { return this.context; }
        }

        

        //public static ParseResultCollection Parse(string text)
        //{
        //    return ParseResultCollection.InternalParse(text,new PlaceNameParser(text));
        //}

        public PlaceNameParser(ParserContext context)
        {
            this.context = context;
            if (placeNamesTrie == null)
                placeNamesTrie = context.GetServerInstance();

            placeNamesTrie.Connect(context.ServerEndPoint);
        }

        internal string MatchPlaceName(string text, int startIndex)
        {
            int maxlength = maxChinesePlaceNameLength;
            string temp = text.Substring(startIndex,Math.Min(text.Length - startIndex,maxlength));

            var result = placeNamesTrie.MaximumMatch(temp, (int)POSType.A_NS);
            if(result==null)
                return null;
             return result.Word;
        }

        #region IParser Members

        public ParseResultCollection Parse(int startIndex)
        {
            string _text = context.Text;

            ParseResultCollection prc = new ParseResultCollection();   
            string placeName = MatchPlaceName(_text, startIndex);
            if (placeName != null)
            {
                ParseResult pr = new ParseResult();
                pr.Text = placeName;
                pr.StartPos = startIndex;
                pr.Type = POSType.A_NS;
                prc.Add(pr);
                return prc;
            }
            return prc;
        }

        #endregion
    }
}
