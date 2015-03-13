using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Nepy.Configuration
{
    [ConfigurationCollection(typeof(ParserAssemblyLoaderSetting), AddItemName = "parserLoader")]
    public class ParserAssemblyLoaderCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ParserAssemblyLoaderSetting();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ParserAssemblyLoaderSetting)element).Path;
        }
    }

}
