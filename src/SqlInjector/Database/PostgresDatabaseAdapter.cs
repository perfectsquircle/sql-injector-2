﻿using System.Data.Common;

using Npgsql;

using SqlInjector.Model;

namespace SqlInjector.Database;

public class PostgresDatabaseAdapter : IDatabaseAdapter
{
    private NpgsqlDataSource _dataSource;

    public void Connect(ConnectPayload connectPayload)
    {
        var connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = connectPayload.Host,
            Port = connectPayload.Port,
            Database = connectPayload.Database,
            Username = connectPayload.Username,
            Password = connectPayload.Password,
            Enlist = false,
            Pooling = false,
        };

        _dataSource = NpgsqlDataSource.Create(connectionString.ConnectionString);
    }

    public void Dispose()
    {
        ((IDisposable)_dataSource)?.Dispose();
    }

    public async Task<QueryResults> QueryAsync(QueryPayload queryPayload)
    {
        using var command = _dataSource.CreateCommand(queryPayload.Sql);
        using var results = await command.ExecuteReaderAsync();

        if (results.RecordsAffected != -1)
        {
            return new QueryResults(results.RecordsAffected);
        }

        var columnSchema = await results.GetColumnSchemaAsync();
        var columns = columnSchema.Select(c => new Column(c.ColumnName, c.DataTypeName));

        return new QueryResults(columns, ReadRows(results));
    }

    private IEnumerable<object[]> ReadRows(DbDataReader results)
    {
        while (results.Read())
        {
            var row = new object[results.FieldCount];
            results.GetValues(row);
            yield return row;
        }
    }
}