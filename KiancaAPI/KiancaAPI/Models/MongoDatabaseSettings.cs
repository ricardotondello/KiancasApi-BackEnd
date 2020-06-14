namespace KiancaAPI.Models
{
    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {
        public string PersonCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get ; set ; }
    }

    public interface IMongoDatabaseSettings
    {
        string PersonCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
