using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ParserDefaultOrderAttribute : Attribute
    {
        public ParserDefaultOrderAttribute(int order) 
        {
            this.Order = order;
        }
        public int Order { get; set; }
    }
}
