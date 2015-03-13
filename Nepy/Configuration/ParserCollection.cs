using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Nepy.Configuration
{
    [ConfigurationCollection(typeof(ParserAssemblyLoaderSetting), AddItemName = "parser")]
    public class ParserCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ParserSetting();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ParserSetting)element).Name;
        }
    }
}
