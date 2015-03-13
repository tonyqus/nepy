using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Nepy.Configuration
{
    public class ParserAssemblyLoaderSetting : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path
        {
            get { return (string)(base["path"]); }
            set { base["path"] = value; }
        }
    }
}
