using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Nepy.Configuration
{
    public class ParserSetting : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)(base["name"]); }
            set { base["name"] = value; }
        }
        [ConfigurationProperty("order", IsRequired = false)]
        public int Order
        {
            get { return (int)(base["order"]); }
            set { base["order"] = value; }
        }
        //[ConfigurationProperty("dictionaryServiceName", IsRequired = false)]
        //public string DictionaryServiceName
        //{
        //    get { return (string)(base["dictionaryServiceName"]); }
        //    set { base["dictionaryServiceName"] = value; }
        //}
    }
}
