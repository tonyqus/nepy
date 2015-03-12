using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BluePrint.NLPCore;
using BluePrint.Dictionary;
using System.Text.RegularExpressions;

namespace BluePrint.SegmentFramework.Parser
{
    [ParserOrder(20)]
    public class OrgNameParser:IParser
    {
        public const int maxChineseOrgNameLength = 16;
        public const int maxMiddlePartLength = 10;
        public static DictionaryServiceClient orgNamesTrie = null;

        string[] suffixList = { "公司", "集团", "银行","中心","企业",
                                  "科技", "软件", "信息", "咨询", "物流", "电子", "教育", "英语", "日语", "管理","航空","出版社",
                                  "学院", "大学", "中学", "小学", "幼儿园",
                                  "医院","广告", "传媒", "杂志",  
                                  "超市", "便利店", "网络","百货","酒店", "旅馆", "饭店", 
                                  "厂","所", "部", "院", "委","局" };
        #region IParser Members

        ParserContext context;
        public OrgNameParser(ParserContext context)
        {
            this.context = context;
            orgNamesTrie = context.GetServerInstance();
            orgNamesTrie.Connect(context.ServerEndPoint);
        }
        public ParserContext Context
        {
            get { return this.context; }
        }

        string MatchOrgName(string text, int startIndex)
        {
            int maxlength = 10;
            string temp = text.Substring(startIndex, Math.Min(text.Length - startIndex, maxlength));

            var result = orgNamesTrie.MaximumMatch(temp, (int)POSType.A_NT);
            if (result == null)
                return null;
            return result.Word;
            //for (int j = Math.Min(text.Length - startIndex, maxlength); j > 1; j--)
            //{
            //    string temp = text.Substring(startIndex, j);
            //    int freq = orgNamesTrie.GetFrequency(temp);
            //    if (freq <= 0)
            //        continue;
            //    else
            //        return temp;
            //}
            //return null;
        }

        //public static ParseResultCollection Parse(string text)
        //{
        //    return ParseResultCollection.InternalParse(text, new OrgNameParser(text));
        //}
        
        public ParseResultCollection Parse(int startIndex)
        {
            string _text = context.Text;
            ParseResultCollection prc = new ParseResultCollection();
            string temp = _text.Substring(startIndex, Math.Min(maxChineseOrgNameLength,_text.Length-startIndex));
            int pos = -1;
            string suffix = null;
            for (int i = 0; i < suffixList.Length; i++)
            {
                pos = temp.IndexOf(suffixList[i]);
                if(pos>0)
                {
                    suffix = suffixList[i];
                    break;
                }
            }
            if (pos <= 0)    //找不到后缀，直接返回
                return prc;
            //寻找前置地名
            string placeName = null;
            ParserContext context1 = this.context.Clone();
            context1.Text = temp;
            IParser placeNameParser = new PlaceNameParser(context1);
            ParseResultCollection prc1 = placeNameParser.Parse(0);
            if (prc1.Count > 0)
            {
                placeName = (string)prc1[0].Text;
            }

            if (placeName!=null && pos -placeName.Length < maxMiddlePartLength)
            {
                prc.Add(ParseResult.Create(temp.Substring(0, pos + suffix.Length), startIndex, POSType.A_NT));
            }
            else if (context.Text.IndexOf("（")>0)
            {
                int bracePos = context.Text.IndexOf("（");

                IParser placeNameParser2 = new PlaceNameParser(context);
                ParseResultCollection prc2 = placeNameParser2.Parse(bracePos+1);
                if (prc2.Count > 0)
                {
                    placeName = (string)prc2[0].Text;
                    prc.Add(ParseResult.Create(temp.Substring(0, pos + suffix.Length), startIndex, POSType.A_NT));
                }
            }
            else
            {
                //没有找到地名
                string orgName = MatchOrgName(temp, 0);
                if (orgName != null)
                {
                    prc.Add(ParseResult.Create(orgName, startIndex, POSType.A_NT));
                }
                else
                {
                    //库中没有，使用谓词定位边界                    
                }
            }
            
            return prc;
            /*
             * 《现代汉语词汇研究-中文信息处理》
             确定规则
             * a. 如果候选地名字符串前一词为地名指界词，且候选地名字串后一个词为地名特征词，则候选地名左右边界确定
             * b. 如果候选地名字符串前一词为地名指界词，则候选地名左边界确定
             * c. 如果候选地名字串后一个词为地名指界词，则候选地名右边界确定
             * d. 如果两个候选地名字串存在并列关系， 其中一个候选地名被确定，则另一个候选地名也被确定
             否定规则
             * 称谓词否定规则：如果候选地名字串的前一词是人名称谓词，且候选地名字串中没有地名特征词，否定该地名字串。
             * 指界词否定规则：如果候选地名字串的后一词为人名指界词，且候选地名字串中没有地名特征词，否定该地名字串。
             * 并列否定规则：如果两个候选地名字串存在并列关系，其中一个候选地名被否定，另一个候选地名也被否定。
             * 其他物体类否定规则：如果候选地名字符串的后一词为其他物体类特征词，否定该地名字串。如红塔山香烟
             * 非单字词否定规则：如果候选地名字串的前一词不是单字词，或候选地名字串的后一词不是单字词，则否定候选地名
             边界修正规则
             * 称谓词与特征词修正规则：如果候选地名字串的前一词为人名称谓词且候选地名字串中存在地名特征词，则修正地名的边界
             */
        }

        #endregion
    }
}
