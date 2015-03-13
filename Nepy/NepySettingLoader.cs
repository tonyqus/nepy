using Nepy.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Nepy
{
    public class NepySettingLoader
    {
        private static NepySetting setting = null;
        private NepySettingLoader() { }

        public static NepySetting GetSetting()
        {
            if(setting==null) 
                setting=ConfigurationManager.GetSection("nepy") as NepySetting;
            return setting;
        }
    }
}
