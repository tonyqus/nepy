using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Core
{
    public class PhoneNo
    {
        public string CountryCode
        { get; set; }
        public string AreaCode 
        { get; set; }
        public string Main
        { get; set; }
        public string Extension
        { get; set; }
        public bool IsMobile
        { get; set; }
    }
}
