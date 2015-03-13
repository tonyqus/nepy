using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Core
{
    public class PhoneNo
    {
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string Main { get; set; }
        public string Extension { get; set; }
        public bool IsMobile { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (CountryCode != null)
            {
                sb.Append(CountryCode);
                sb.Append(" ");
            }
            if (AreaCode != null)
            {
                sb.Append(AreaCode);
                sb.Append(" ");
            }
            sb.Append(Main);
            if (Extension != null)
            {
                sb.Append("#");
                sb.Append(Extension);
            }
            return sb.ToString();
        }
    }
}
