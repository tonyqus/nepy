using Nepy.Core;

namespace Nepy.Dictionary
{
    public interface IDataNode
    {
        /// <summary>
        /// word text
        /// </summary>
        string Word 
        { 
            get; set; 
        }
        /// <summary>
        /// Part of Speech Tag Value
        /// </summary>
        POSType POS 
        { 
            get; set; 
        }
        /// <summary>
        /// frequency of the word
        /// </summary>
        double Frequency
        { 
            get; set; 
        }
    }
}
