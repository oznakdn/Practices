namespace CrudWithMongoDb.Data
{
    public interface IDatabaseSetting
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string StudentCoursesCollectionName { get; set; }
    }
}
