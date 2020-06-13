namespace KiancaAPI.Models
{
    public class PersonstoreDatabaseSettings : IPersonstoreDatabaseSettings
    {
        public string PersonCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get ; set ; }
    }

    public interface IPersonstoreDatabaseSettings
    {
        string PersonCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
