namespace SqlInjector.Database;

public interface IDatabaseAdapterFactory
{
    IDatabaseAdapter GetDatabaseAdapter(string databaseType);
}

public class DatabaseAdapterFactory : IDatabaseAdapterFactory
{
    public IDatabaseAdapter GetDatabaseAdapter(string databaseType)
    {
        return databaseType switch
        {
            "postgres" => new PostgresDatabaseAdapter(),
            _ => throw new NotImplementedException(),
        };
    }
}
