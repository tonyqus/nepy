using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Nepy.Core;

namespace Nepy.Dictionary.Providers
{
    public class TxtFileProvider:IDataProvider
    {
        IDataProviderSetting setting = null;
        public TxtFileProvider(IDataProviderSetting setting)
        {
            this.setting = setting;
        }
        public List<IDataNode> Load()
        {
            List<IDataNode> nodes = new List<IDataNode>();
            using (StreamReader sr = new StreamReader(setting.Uri))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] parts = line.Split(new char[] { ' ' });
                    WordAttribute wa = new WordAttribute();
                    if (parts.Length == 3)
                    {
                        wa.Word = parts[0];
                        wa.Frequency = Double.Parse(parts[1]);
                        wa.POS = (POSType)Convert.ToInt32(parts[2]);
                    }
                    else
                    {
                        wa.Word = parts[0];
                    }
                    nodes.Add(wa);
                    line = sr.ReadLine();
                }
            }
            return nodes;
        }
    }
}
