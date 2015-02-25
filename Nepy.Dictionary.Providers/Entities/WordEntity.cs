using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nepy.Core;

namespace Nepy.Dictionary.Providers
{
    public class WordEntity:IDataNode
    {
        public ObjectId id { get; set; }
        [BsonElement("text")]
        public string Word { get; set; }
        [BsonElement("freq")]
        public double Frequency { get; set; }
        public string pinYinString { get; set; }
        public int textLength { get; set; }
        public string scelSource { get; set; }
        public string catgory { get; set; }

        public override string ToString()
        {
            return string.Format("{0},freq:{1}", this.Word, this.Frequency);
        }
        [BsonIgnore]
        public POSType POS
        {
            get;
            set;
        }
    }
}
