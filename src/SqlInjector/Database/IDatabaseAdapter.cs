using SqlInjector.Model;

namespace SqlInjector.Database;

public interface IDatabaseAdapter : IDisposable
{
    void Connect(ConnectPayload connectPayload);
    Task<QueryResults> QueryAsync(QueryPayload queryPayload);
}
