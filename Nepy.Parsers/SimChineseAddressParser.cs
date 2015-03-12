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
    [ParserOrder(60)]
    public class ChineseAddressParser:IParser
    {
        public const int maxChineseAddressLength = 50;
        ParserContext context;
        //MongoCollection<CityEntity> _cityNames;
        //MongoCollection<NameEntity> _placeNames;
        DictionaryServiceClient client;

        public ParserContext Context
        {
            get { return this.context; }
        }

        public ChineseAddressParser(ParserContext context)
        {
            if (context.Pattern != ParserPattern.China)
                throw new ArgumentOutOfRangeException("中文地址仅对ParserPattern.China有效");
            this.context = context;
            client = context.GetServerInstance();

            client.Connect(context.ServerEndPoint);
            //MongoDatabase db = server.GetDatabase("nameResearch");
            //_cityNames = db.GetCollection<CityEntity>("cityNames");
            //_placeNames = db.GetCollection<NameEntity>("placeNames");
        }
        //internal static string GetMaximumMatch(string text, int startIndex, int maxlength, string fieldname, MongoCollection<CityEntity> dbcol, IMongoQuery query)
        //{
        //    string word = null;
        //    int j;
        //    string temp = null;
        //    for (j = maxlength; j > 0; j--)
        //    {
        //        temp = text.Substring(startIndex, j + startIndex > text.Length ? text.Length - startIndex : j);
        //        long c2 = 0;
        //        if (query == null)
        //        {
        //            if (fieldname == "city")
        //            {
        //                c2 = dbcol.Count(Query.Or(Query.EQ(fieldname, temp), Query.EQ(fieldname, temp + '市')));
        //            }
        //            else
        //            {
        //                c2 = dbcol.Count(Query.EQ(fieldname, temp));
        //            }
        //        }
        //        else
        //        {
        //            c2 = dbcol.Count(Query.And(Query.EQ(fieldname, temp), query));
        //        }
        //        if (c2 > 0)
        //        {
        //            word = temp;
        //            //if (fieldname == "city" && !temp.EndsWith("市"))
        //            //{
        //            //    word += '市';
        //            //}
        //            return word;
        //        }
        //    }
        //    return null;
        //}

        internal string GetMaximumMatch(string text, int startIndex, int maxlength)
        {
            string temp = text.Substring(startIndex,  Math.Min(text.Length - startIndex, maxlength));
            var result = client.MaximumMatch(temp, (int)POSType.A_NS);
            if (result == null)
                return null;
            return result.Word;
        }
        #region IParser Members

        public ParseResultCollection Parse(int startIndex)
        {
            string _text = context.Text;
            ParseResultCollection prc = new ParseResultCollection();
            string temp = _text.Substring(startIndex, Math.Min(maxChineseAddressLength, _text.Length - startIndex));
            char[] chars = temp.ToCharArray();
            //int lastStartPos = 0;
            StringBuilder sb = new StringBuilder();
            StringBuilder whole = new StringBuilder();
            ChineseAddress ca=new ChineseAddress();
            int startpos = 0;
            //TODO: 通过字典找国家名
            if (temp.StartsWith("中国"))
            {
                startpos = 2;
                ca.country = "中国";
                whole.Append("中国");
            }
            for (int i = startpos; i < chars.Length; i++)
            {
                char ch = chars[i];
                if (ch == '市'||ch=='场')
                {
                    
                    if (sb.Length == 0)
                    {
                        sb.Append(ch);
                        continue;
                    }
                    sb.Append(ch);
                    string subStr = sb.ToString();
                    string city = GetMaximumMatch(subStr,0,5);
                    if (city != null)
                    {
                        ca.city = city;
                        whole.Append(ca.city);
                        sb = new StringBuilder();
                    }
                }
                else if (ch == '区')
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(ch);
                        continue;
                    }
                    sb.Append(ch);
                    string subStr = sb.ToString();
                    
                    string district = GetMaximumMatch(subStr, 0, 5);
                    if (district != null)
                    {
                        if (!district.EndsWith("区"))
                        {
                            ca.city = district;
                            whole.Append(ca.city);
                            ca.district = subStr.Substring(ca.city.Length);
                            whole.Append(ca.district);
                        }
                        else
                        {
                            //string district = NEParser.GetMaximumMatch(subStr, 0, 5, "district", _cityNames, null);
                            ca.district = district; 
                            whole.Append(ca.district);
                        }
                    }
                    else
                    {
                        ca.district = subStr;
                        whole.Append(ca.district);
                    }
                    sb = new StringBuilder();
                }
                else if (ch == '省')
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(ch);
                        continue;
                    }
                    sb.Append(ch);
                    string subStr = sb.ToString();
                    string province = GetMaximumMatch(subStr, 0, 5);    //省份
                    if (province != null)
                    {
                        ca.province = province;
                        whole.Append(ca.province);
                        sb = new StringBuilder();
                    }
                }
                else if (ch == '乡' || ch == '村' || ch == '县' || ch == '镇')
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(ch);
                        continue;
                    }
                    sb.Append(ch);
                    ca.county = sb.ToString();
                    whole.Append(ca.county);
                    sb = new StringBuilder();
                }
                else if (ch == '巷')
                { 
                    
                }
                else if (ch == '楼'||ch == '弄'||ch == '号'||ch == '室')
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(ch);
                        continue;
                    }
                    string substr = NumeralUtil.ConvertChineseNumeral2Arabic(sb.ToString());
                    int x;
                    sb.Append(ch);
                    if (Int32.TryParse(substr, out x))
                    {
                        if (ch == '楼')
                            ca.floor = sb.ToString();
                        else if (ch == '弄')
                            ca.lane = sb.ToString();
                        else if (ch == '号')
                            ca.no = sb.ToString();
                        else if (ch == '室')
                            ca.room = sb.ToString();
                        whole.Append(sb.ToString());
                        sb = new StringBuilder();
                    }
                }
                else if (ch == '道' || ch == '路' || ch == '街')
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(ch);
                        continue;
                    }
                    sb.Append(ch);
                    ca.street = sb.ToString();
                    whole.Append(ca.street);
                    sb = new StringBuilder();
                }
                else if (ch == '（' || ch == '(')
                {
                    sb = new StringBuilder();
                    sb.Append(ch);
                }
                else if (ch == '）' || ch == ')')
                {
                    sb.Append(ch);
                    string extra1 = sb.ToString();
                    whole.Append(extra1);
                    ca.extra = extra1;
                    sb = new StringBuilder();
                }
                else if (CharacterUtil.IsChinesePunctuation(ch) || (ch == ' ' || ch == '　'))
                {
                    break;
                }
                else if (ch == '大')
                { 
                    if (sb.Length == 0)
                    {
                        sb.Append(ch);
                        continue;
                    }
                    if (i + 1 < chars.Length)
                    {
                        char nextchar = chars[i + 1];
                        
                        if (nextchar == '桥' || nextchar == '厦')
                        {
                            string extra1 = sb.ToString() + "大" + nextchar;
                            whole.Append(extra1);
                            if (nextchar == '桥')
                                ca.extra += extra1;
                            else
                                ca.building = extra1;
                            i += 2-1;
                            sb = new StringBuilder();
                        }
                        else if (i + 2 < chars.Length && nextchar == '酒')
                        {
                            char nextchar2 = chars[i + 2];

                            if (nextchar2 == '店')
                            {
                                string extra1 = sb.ToString() + "大" + nextchar+ nextchar2;
                                string city = GetMaximumMatch(extra1, 0, 5);    //城市或省份
                                if (city != null)
                                {
                                    ca.city = city;
                                    whole.Append(ca.city);

                                    extra1 = extra1.Substring(ca.city.Length);
                                }
                                whole.Append(extra1);
                                ca.building= extra1;
                                i += 3-1;
                                sb = new StringBuilder();
                            }
                        }
                    }
                }
                else if(ch=='餐')
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(ch);
                        continue;
                    } 
                    if (i + 1 < chars.Length)
                    {
                        char nextchar = chars[i + 1];
                        if (nextchar == '厅')
                        {
                            string extra1 = sb.ToString() + "餐" + nextchar;
                            whole.Append(extra1);
                            ca.extra += extra1;
                            i += 2 - 1;
                            sb = new StringBuilder();
                        }
                    }
                }
                else
                {
                    //if (sb.Length == 0)
                    //    lastStartPos = i;
                    sb.Append(ch);
                    string extra = sb.ToString();
                    if (extra.EndsWith("中心") || extra.EndsWith("酒店"))
                    {
                        string city = GetMaximumMatch(extra, 0, 5); //城市
                        if (city != null)
                        {
                            ca.city = city;
                            extra = extra.Substring(city.Length);

                        }
                        ca.building = extra;
                        whole.Append(extra);
                        if (i + 2 < chars.Length && chars[i + 1] == '大' && chars[i + 2] == '厦')  //处理 "中心大厦"
                        {
                            ca.building += "大厦";
                            whole.Append("大厦");
                            i += 2;
                            sb = new StringBuilder();
                            continue;
                        }
                        sb = new StringBuilder();
                    }
                }
                
            }
            if ( whole.Length>0)
            {
                if(sb.Length>0)
                    ca.extra= sb.ToString();
                prc.Add(ParseResult.Create(whole.ToString(), startIndex, POSType.D_S,ca));
            }
            return prc;
        }

        #endregion
    }
}
