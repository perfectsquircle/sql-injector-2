namespace SqlInjector.Model;

public class QueryResults
{
    public IEnumerable<Column> Columns { get; }
    public IEnumerable<object[]> Rows { get; }
    public int RecordsAffected { get; }

    public QueryResults(IEnumerable<Column> columns, IEnumerable<object[]> rows)
    {
        Columns = columns;
        Rows = rows;
    }

    public QueryResults(int recordsAffected)
    {
        RecordsAffected = recordsAffected;
    }
}

public class Column
{
    public string Name { get; set; }
    public string DataType { get; set; }

    public Column(string name, string dataType)
    {
        Name = name;
        DataType = dataType;
    }
}
