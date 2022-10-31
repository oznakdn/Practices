namespace CrudWithMongoDb.Data
{
    public class DatabaseSetting : IDatabaseSetting
    {
        public string ConnectionString { get; set ; } = string.Empty;
        public string DatabaseName { get ; set ; }=string.Empty;
        public string StudentCoursesCollectionName { get ; set; } = string.Empty;
    }
}
