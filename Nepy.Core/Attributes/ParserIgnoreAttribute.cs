using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nepy.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ParserIgnoreAttribute : Attribute
    {
        public ParserIgnoreAttribute()
        {
        }
    }
}
