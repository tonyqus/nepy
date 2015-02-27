using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Reflection;

namespace Nepy.Dictionary.Providers
{
    public class MongoDBDataProvider:IDataProvider
    {
        Type entityType = null;
        IDataProviderSetting setting = null;

        internal Type LoadEntityType(string assemblyString)
        {
            if (string.IsNullOrEmpty(assemblyString))
                throw new ArgumentNullException("entityType property cannot be empty");

            string[] splits = assemblyString.Split(new char[] { ','});
            if (splits.Length != 2)
            {
                throw new ArgumentOutOfRangeException("entityType is not a valid assembly reference string");
            }
            string assemblyName = splits[1];
            string className = splits[0];
            return Assembly.Load(assemblyName).GetType(className);
        }

        public MongoDBDataProvider(IDataProviderSetting setting)
        {
            if (string.IsNullOrEmpty(setting.EntityType))
            {
                this.entityType = typeof(WordEntity);
            }
            else
            {
                this.entityType = LoadEntityType(setting.EntityType);
            }
            this.setting = setting;
        }
        #region IDataProvider Members

        public List<IDataNode> Load()
        {
            var client=new MongoClient(setting.Uri);
            var server=client.GetServer();
            try
            {
                server.Connect();
                MongoDatabase db = server.GetDatabase(setting.DBName);
                var companiesDB = db.GetCollection(setting.CollectionName);
                var cursor = companiesDB.FindAllAs(entityType);
                if (!string.IsNullOrEmpty(setting.IncludeFields))   //todo:fix includefields bug for orgname
                {
                    string[] fields = setting.IncludeFields.Split(new char[] { ',' });
                    cursor.SetFields(Fields.Include(fields));
                }
                List<IDataNode> list = cursor.Cast<IDataNode>().ToList();
                return list;
            }
            finally
            {
                server.Disconnect();
            }
        }

        #endregion
    }
}
