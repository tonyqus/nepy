using System.Collections.Generic;

namespace Nepy.Dictionary
{
    public interface IDataProvider
    {
        List<IDataNode> Load();
    }
}
