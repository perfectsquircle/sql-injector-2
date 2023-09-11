using SqlInjector.Model;

namespace SqlInjector.Database;

public interface IDatabaseAdapter : IDisposable
{
    void Connect(ConnectPayload connectPayload);
    QueryResults Query(QueryPayload queryPayload);
}
