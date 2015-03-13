using Nepy.Configuration;
using Nepy.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nepy
{
    public class NamedEntityRecognizer
    {
        NepySetting setting = null;
        List<ConstructorInfo> parserConstructors = null;

        public NamedEntityRecognizer()
        {
            setting = NepySettingLoader.GetSetting();
            InitializeParsers();
        }
        public ParseResultCollection Recognize(string text, ParserPattern pattern)
        {
            ParserContext context = new ParserContext();
            context.Pattern = pattern;
            context.Text = text;

            ParseResultCollection result = new ParseResultCollection();

            char[] chars = text.ToCharArray();

            int i = 0;

            while (i < chars.Length)
            {
                char c = chars[i];

                if (CharacterUtil.IsChinesePunctuation(c))
                {
                    i++;
                    continue;
                }
                bool isFound = false;
                //扫描地名（优先于姓名，用于排除不正确人名）
                foreach (ConstructorInfo ci in parserConstructors)
                {
                    IParser parser = ci.Invoke(new object[] { context }) as IParser;

                    try
                    {
                        ParseResultCollection prc = parser.Parse(i);

                        if (prc.Count > 0)
                        {
                            foreach (ParseResult pr in prc)
                            {
                                result.Add(pr);
                                i += pr.Length;
                            }
                            isFound = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                if (!isFound)
                {
                    i++;
                }
            }
            return result;
        }

        private void InitializeParsers()
        {
            if (parserConstructors == null)
            {
                FindParsers();
                parserConstructors = new List<ConstructorInfo>();
                foreach (Type type in parserTypeList.Values)
                {
                    ConstructorInfo ci = type.GetConstructor(new Type[] { typeof(ParserContext) });
                    parserConstructors.Add(ci);
                }
            }
        }
        Dictionary<int, Type> parserTypeList = null;
        internal void FindParsers()
        {
            if (parserTypeList != null)
                return;
            parserTypeList = new Dictionary<int, Type>();
            foreach (ParserAssemblyLoaderSetting pls in setting.ParserAssemblies)
            {
                string absolutePath = Path.GetFullPath(pls.Path);
                Assembly parserAssembly = Assembly.LoadFile(absolutePath);
                Type[] types = parserAssembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    Type type = types[i];

                    Type[] interfaces = type.GetInterfaces();
                    object[] attributes = type.GetCustomAttributes(false);

                    int order = 0;
                    for (int j = 0; j < interfaces.Length; j++)
                    {
                        if (interfaces[j] == typeof(IParser))
                        {
                            bool isLoad = true;
                            foreach (Attribute attr in attributes)
                            {
                                if (attr is ParserIgnoreAttribute)
                                {
                                    isLoad = false;
                                    break;
                                }
                                else if (attr is ParserDefaultOrderAttribute)
                                {
                                    order = ((ParserDefaultOrderAttribute)attr).Order; //defaultOrder
                                }
                            }
                            if (isLoad)
                            {
                                if (order <= 0)
                                    throw new ArgumentException(string.Format("Parser default order is not set for {0}", type.Name));
                                foreach (ParserSetting ps in setting.Parsers)
                                {
                                    if (ps.Name == type.Name && ps.Order > 0)
                                    {
                                        order = ps.Order;
                                        break;
                                    }
                                }
                                if (parserTypeList.ContainsKey(order))
                                    throw new ArgumentException(string.Format("Parser order {0} conflicts for '{1}'", order, type.Name));
                                parserTypeList.Add(order, type);
                            }
                            break;
                        }
                    }
                }
            }
            parserTypeList = (from parser in parserTypeList
                              orderby parser.Key ascending
                              select parser).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

    }
}
