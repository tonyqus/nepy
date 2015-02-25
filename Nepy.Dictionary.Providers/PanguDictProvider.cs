using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Dictionary.Providers
{
    public class PanguDictProvider:IDataProvider
    {
        IDataProviderSetting setting = null;
        public PanguDictProvider(IDataProviderSetting setting)
        {
            this.setting = setting;
        }
        public List<IDataNode> Load()
        {
            WordDictionary wd = new WordDictionary();
            wd.Load(setting.Uri);
            return wd.WordDict;
        }
    }
}
