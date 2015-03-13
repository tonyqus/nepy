using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Nepy.Configuration
{
    public class NepySetting : ConfigurationSection
    {
        [ConfigurationProperty("parserAssemblies")]
        public ParserAssemblyLoaderCollection ParserAssemblies
        {
            get { return (ParserAssemblyLoaderCollection)this["parserAssemblies"]; }
        }
        [ConfigurationProperty("parsers")]
        public ParserCollection Parsers
        {
            get { return (ParserCollection)this["parsers"]; }
        }

    }
}
