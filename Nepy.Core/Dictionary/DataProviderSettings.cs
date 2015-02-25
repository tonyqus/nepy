using System.Configuration;

namespace Nepy.Dictionary
{
    public class DataProviderSettings : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public DataProviderCollection Instances
        {
            get { return (DataProviderCollection)this[""]; }
        }
    }

    [ConfigurationCollection(typeof(DataProviderSetting), AddItemName = "provider")]
    public class DataProviderCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DataProviderSetting();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DataProviderSetting)element).Name;
        }
    }

    public class DataProviderSetting : ConfigurationElement, IDataProviderSetting
    {
        public DataProviderSetting()
        {

        }
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)(base["name"]); }
            set { base["name"] = value; }
        }
        [ConfigurationProperty("uri", IsRequired = true)]
        public string Uri
        {
            get { return (string)(base["uri"]); }
            set { base["uri"] = value; }
        }
        [ConfigurationProperty("type", IsKey = false, IsRequired = true)]
        public string ProviderType
        {
            get { return (string)(base["type"]); }
            set { base["type"] = value; }
        }
        [ConfigurationProperty("dbname")]
        public string DBName
        {
            get { return this["dbname"].ToString(); }
            set { this["dbname"] = value; }
        }
        [ConfigurationProperty("collectionName")]
        public string CollectionName
        {
            get { return this["collectionName"].ToString(); }
            set { this["collectionName"] = value; }
        }
        [ConfigurationProperty("entityType")]
        public string EntityType
        {
            get { return this["entityType"].ToString(); }
            set { this["entityType"] = value; }
        }
        [ConfigurationProperty("includeFields")]
        public string IncludeFields
        {
            get { return this["includeFields"].ToString(); }
            set { this["includeFields"] = value; }
        }
    }
}
