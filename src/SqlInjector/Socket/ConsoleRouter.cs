using SqlInjector.Database;
using SqlInjector.Model;

namespace SqlInjector.Socket;

public class ConsoleRouter : Router, IDisposable
{
    private readonly IDatabaseAdapterFactory _databaseAdapterFactory;
    private IDatabaseAdapter _databaseAdapter;

    public ConsoleRouter()
    {
        _databaseAdapterFactory = new DatabaseAdapterFactory();
    }

    [Procedure]
    public void Connect(ConnectPayload connectPayload)
    {
        _databaseAdapter = _databaseAdapterFactory.GetDatabaseAdapter(connectPayload.DatabaseType);
        _databaseAdapter.Connect(connectPayload);
    }

    [Procedure]
    public QueryResults Query(QueryPayload queryPayload)
    {
        if (_databaseAdapter is null)
        {
            throw new NotSupportedException("Attempting to query before connect.");
        }
        var results = _databaseAdapter.Query(queryPayload);
        return results;
    }

    public void Dispose()
    {
        _databaseAdapter?.Dispose();
    }
}
