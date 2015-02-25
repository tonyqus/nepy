using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nepy.Core;

namespace Nepy.Dictionary.Providers.Entities
{
    public class NameEntity:IDataNode 
    {
        public ObjectId id;
        public NameEntity()
        {
            preWord = new List<string>();
            postWord = new List<string>();
        }
        [BsonElement("name")]
        public string Word { get; set; }
        [BsonElement("freq")]
        public double Frequency { get; set; }
        public int length;
        public List<string> preWord;
        public List<string> postWord;

        [BsonIgnore]
        public POSType POS
        {
            get
            {
                return POSType.A_NR;
            }
            set
            {
               
            }
        }

        public override string ToString()
        {
            return Word;
        }
    }
}
