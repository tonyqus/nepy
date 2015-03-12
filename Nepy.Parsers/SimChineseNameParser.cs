using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BluePrint.SegmentFramework;
using BluePrint.NLPCore;
using BluePrint.Dictionary.Entities;
using BluePrint.Dictionary;

namespace BluePrint.SegmentFramework.Parser
{
    /// <summary>
    /// 中文人名识别
    /// </summary>
    [ParserOrder(25)]
    public class NameParser:IParser
    {

        //中文称谓 http://www.hudong.com/wiki/%E7%A7%B0%E8%B0%93%E7%A4%BC%E4%BB%AA
        /// <summary>
        /// 前缀称谓列表， 如老陈
        /// </summary>
        List<string> prefixTitleList = new List<string>()
        {
            "大", "老", "小","剧作家", "作曲家", "影星", "表妹", "前妻", "养女", "大学生", "群众", "团员", "党员", "职员",
            "未婚妻","同伴", "室友", "好友", "主人","运动员","作家", "售货员","搬运工","临时工","退伍军人", "记者", "艺术家"
        };
        /// <summary>
        /// 前缀或后缀称谓列表
        /// </summary>
        List<string> titleList = new List<string>()
        {
            "侦查员", "旅长", "军长", "连长", "师长", "司令", "排长", "团长", "上尉", "中校", "少校", "上校", "司令官", "将军",
            "老板","工程师","总管", "公公","主席", "总理", "委员","书记","总统", "国务卿",
            "医生", "护士","医师","大夫", "老师", "教授",  "会计", "警官", 
            "局长","科长", "经理", "店长", "处长", "院长", "代表", "厂长",  "总监", "总督", 
            "市长","省长", "县长", "区长", "设计师",  "师傅",  "同学", "村长", "总指挥","清洁员", "保洁员",  "助理", "秘书", "司机", 
            "男子", "女子"
        };
        /// <summary>
        /// 后缀称谓列表， 如张小姐
        /// </summary>        
        List<string> suffixTitleList = new List<string>() 
        { 
            "先生", "女士", "小姐", "少爷", "大人", "夫子","太太",
            "叔叔", "阿姨", "大爷", "老伯","伯伯", "爷爷", "奶奶",
            "总", "工", "同志"  
        };
        //http://tieba.baidu.com/f?kz=765543169
        //http://www.yykf.net/xingming/xm78.html
        char[] avoidChars = new char[] { 
            '婊', '娼', '妓', '奸', 
            '爷', '妈', '爸', '孙','叔', //称谓
            '蝇', '虱', '臭','鼠','狼', '豺','猪','鸡','鸭','蛇','蝎', '虫', '蚝','狐', '虮','狗',  //动物
            '牢', '篱', '疥','疵','痤','彘', 
            '灾','害','凶','难','疯','疮','殃','脏','屎','屁','缺','祸',
            '贼','贱','寒','弃','死', '亡', '尸', '粪', '痿'
        };
        public List<string> prefixAndstandard = new List<string>();
        public List<string> suffixAndstandard = new List<string>();
        ParserContext context;
        DictionaryServiceClient client;

        public NameParser(ParserContext context)
        {
            this.context = context;
            client = context.GetServerInstance();
            client.Connect(context.ServerEndPoint);

            prefixAndstandard.AddRange(prefixTitleList);
            prefixAndstandard.AddRange(titleList);
            suffixAndstandard.AddRange(suffixTitleList);
            suffixAndstandard.AddRange(titleList);

        }

        public const int maxChineseNameLength = 5;

        string MatchSurname(string text, int startIndex)
        {
            string temp = text.Substring(startIndex, startIndex+2>text.Length?text.Length-startIndex:2);
            var result = client.MaximumMatch(temp, (int)POSType.A_NR);
            if(result==null)
                return null;

            return result.Word;
        }
        string MatchGivenname(string text, int startIndex)
        {
            //return BluePrint.Dictionary.Utility.GetMaximumMatch(text, startIndex, 3, "name", dbcol);
            return null;
        }
        string MatchFullname(string text, int startIndex)
        {
            string temp = text.Substring(startIndex,5);
            var result = client.MaximumMatch(temp, (int)POSType.A_NR);
            if (result == null)
                return null;
            return result.Word;
        }
        const int MaximumGivennameLength = 3;
        /// <summary>
        /// 匹配动词或介词
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startIndex"></param>
        /// <param name="resultStartPos"></param>
        /// <returns></returns>
        bool MatchVerb(string text, int startIndex, out int resultStartPos)
        {
            TrieTreeResult result = null;
            resultStartPos = -1;
            for (int i = startIndex; i < startIndex + MaximumGivennameLength; i++)
            {
                int x = 3;
                if (i + 3 > text.Length)
                {
                    x = text.Length - i;
                }
                string temp = text.Substring(i, x);    //这里3是最大动词长度
                result = client.MaximumMatch(temp, (int)POSType.D_V|(int)POSType.D_P);
                if (result != null)
                {
                    resultStartPos = i;
                    return true;                    
                }
            }
            return false;
        }
        string MatchPrefix(string text, int startIndex)
        {
            string result = BluePrint.Dictionary.Utility.GetMaximumMatch(text, startIndex, 4, prefixAndstandard);
            return result;
        }
        string MatchSuffix(string text, int startIndex, out int resultStartPos)
        {
            string result = null;
            resultStartPos = -1;
            for (int i = startIndex; i < startIndex + MaximumGivennameLength; i++)
            {
                result = BluePrint.Dictionary.Utility.GetMaximumMatch(text, i, 4, suffixAndstandard);
                if (result != null)
                {
                    resultStartPos = i;
                    return result;
                }
            }
            return null;
        }
        int MatchPunctation(string text, int startIndex, int maxlength)
        {
            for (int i = startIndex; i < startIndex+maxlength; i++)
            {
                if (i+1< text.Length
                    && CharacterUtil.IsChinesePunctuation(text[i+1]))
                {
                    return i+1;
                }
            }
            return -1;
        }

        public ParserContext Context
        {
            get { return this.context; }
        }
        #region IParser Members
        public ParseResultCollection Parse(int startIndex)
        {
            string _text = context.Text;
            ParseResultCollection prc = new ParseResultCollection();
            //TODO:外国人中文姓名处理（无姓）

            //3 找前缀
            string prefix = MatchPrefix(_text, startIndex);
            int prefixlength = 0;
            if (prefix != null)
            {
                prefixlength = prefix.Length;
            }
            //1 扫描百家姓中的姓
            //查单字姓
            int currentPos = startIndex+prefixlength;
            string surname = MatchSurname(_text, currentPos);
            if (surname == null)
            {
                return prc;
            }
            bool surnameInserted = false;
            bool givennameInserted = false;
            if (prefix != null && surname != null)
            {
                prc.Add(ParseResult.Create(prefix, startIndex, POSType.D_N));    //前缀
                surnameInserted = true;
                prc.Add(ParseResult.Create(surname, currentPos, POSType.A_NR));
                currentPos += surname.Length;
            }
            //2 如果姓后面是标点符号，直接认为不是人名
            if (currentPos + 1 < _text.Length
                && CharacterUtil.IsChinesePunctuation(_text[currentPos + 1]))
            {
                return prc;
            }
            //1.1用最大匹配搜索库中的完整人名，如果匹配且权重很高，直接认为是人名
            //string fullname = MatchFullname(_text, startIndex);

            //if (fullname != null)
            //{
            //    prc.Add(ParseResult.Create(surname, startIndex, POSType.A_NR));
            //    prc.Add(ParseResult.Create(fullname.Substring(surname.Length), startIndex + surname.Length, POSType.A_NR));
            //    return prc;
            //}

            //3 找名字   
            //TODO:缩小名字的范围，否则容易造成匹配错误
            //string givenname = MatchGivenname(_text, startIndex + surname.Length);
            //if (givenname != null)
            //{
            //    string suffix2 = MatchSuffix(_text, startIndex + surname.Length + givenname.Length, _siblingWordDB);
            //    if (suffix != null && givenname.Length <= suffix.Length)
            //    {
            //        givenname = null;
            //    }
            //    else
            //    {
            //        suffix = suffix2;
            //    }
            //}
            //4 如果后面是称谓，如先生、小姐、博士、医生，则认为是人名
            int resultStartPos = -1;
            if (surname != null)
            {
                resultStartPos = currentPos + (surnameInserted?0:surname.Length);
                string suffix = MatchSuffix(_text, resultStartPos, out resultStartPos);
                if (suffix != null)
                {
                    if (!surnameInserted)
                    {
                        prc.Add(ParseResult.Create(surname, currentPos, POSType.A_NR));
                        surnameInserted = true;
                        currentPos += surname.Length;
                    }
                    if (resultStartPos > currentPos)
                    {
                        string givenname = _text.Substring(currentPos, resultStartPos - currentPos);
                        prc.Add(ParseResult.Create(givenname, currentPos, POSType.A_NR));
                        prc.Add(ParseResult.Create(suffix, resultStartPos, POSType.D_N));
                        currentPos += givenname.Length + suffix.Length;
                        givennameInserted = true;
                    }
                    else
                    {
                        prc.Add(ParseResult.Create(suffix, currentPos, POSType.D_N));
                        currentPos += suffix.Length;
                    }
                    return prc;
                }
            }

            // 5 如果前面是动词、使动词，可认为是人名
            if (surname != null)
            {
                resultStartPos = currentPos + (surnameInserted ? 0 : surname.Length);
                bool verbFound = MatchVerb(_text, resultStartPos, out resultStartPos);
                if (verbFound && resultStartPos > currentPos + (surnameInserted ? 0 : surname.Length))
                {
                    if (!surnameInserted)
                    {
                        prc.Add(ParseResult.Create(surname, currentPos, POSType.A_NR));
                        surnameInserted = true;
                        currentPos += surname.Length;
                    }
                    if (!givennameInserted)
                    {
                        string givenname = _text.Substring(currentPos, resultStartPos - currentPos);
                        prc.Add(ParseResult.Create(givenname, currentPos, POSType.A_NR));
                        currentPos += givenname.Length;
                        givennameInserted = true;
                    }
                }
            }
            if (surname != null)
            {
                //人名之后直接标点符号， 认为是人名
                int punctuationPos = MatchPunctation(_text, currentPos + (surnameInserted ? 0 : surname.Length), 4);
                if (punctuationPos > 0)
                {
                    if (!surnameInserted)
                    {
                        prc.Add(ParseResult.Create(surname, currentPos, POSType.A_NR));
                        surnameInserted = true;
                        currentPos += surname.Length;
                    }
                    if (!givennameInserted)
                    {
                        string givenname = _text.Substring(currentPos, punctuationPos - currentPos);
                        prc.Add(ParseResult.Create(givenname, currentPos, POSType.A_NR));
                        currentPos += givenname.Length;
                        givennameInserted = true;
                    }
                }
            }
            if (surname != null && _text.Length - currentPos - surname.Length <= MaximumGivennameLength && _text.Length - currentPos - surname.Length>0)  //姓名之后没有字的情况
            {
                if (!surnameInserted)
                {
                    prc.Add(ParseResult.Create(surname, currentPos, POSType.A_NR));
                    surnameInserted = true;
                    currentPos += surname.Length;
                }
                if (!givennameInserted)
                {
                    string givenname = _text.Substring(currentPos, _text.Length - currentPos);
                    prc.Add(ParseResult.Create(givenname, currentPos, POSType.A_NR));
                    currentPos += givenname.Length;
                    givennameInserted = true;
                }
            }
            return prc;
        }

        #endregion
    }
}
