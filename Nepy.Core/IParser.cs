using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace Nepy.Core
{
    public interface IParser
    {
        ParserContext Context { get; }
        ParseResultCollection Parse(int startIndex);
    }
}
