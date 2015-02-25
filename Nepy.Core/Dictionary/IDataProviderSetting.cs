namespace Nepy.Dictionary
{
    public interface IDataProviderSetting
    {
        string Name { get; set; }
        string Uri { get; set; }
        string ProviderType { get; set; }
        string DBName { get; set; }
        string CollectionName { get; set; }
        string EntityType { get; set; }
        string IncludeFields { get; set; }
    }
}
