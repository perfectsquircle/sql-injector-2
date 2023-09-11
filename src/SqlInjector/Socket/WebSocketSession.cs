using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using SqlInjector.Database;
using SqlInjector.Model;

namespace SqlInjector.Socket;

public class WebSocketSession : IDisposable
{
    private readonly WebSocket _webSocket;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly IDatabaseAdapterFactory _databaseAdapterFactory;
    private IDatabaseAdapter _databaseAdapter;

    public WebSocketSession(WebSocket webSocket)
    {
        _webSocket = webSocket;
        _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        _databaseAdapterFactory = new DatabaseAdapterFactory();
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        var buffer = new byte[1024 * 4];
        var receiveResult = await _webSocket.ReceiveAsync(buffer, cancellationToken);

        while (!receiveResult.CloseStatus.HasValue)
        {
            var text = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
            await ConsumeMessageAsync(text, cancellationToken);

            receiveResult = await _webSocket.ReceiveAsync(buffer, cancellationToken);
        }

        await _webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            cancellationToken);
    }

    private async Task ConsumeMessageAsync(string text, CancellationToken cancellationToken)
    {
        var message = JsonSerializer.Deserialize<Message>(text, _jsonSerializerOptions);

        Console.WriteLine(message);

        Response response;
        try
        {
            response = await DispatchAsync(message);
        }
        catch (Exception e)
        {
            response = new Response(Command.Error, e.Message);
        }

        await _webSocket.SendAsync(
             Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response, _jsonSerializerOptions)),
             WebSocketMessageType.Text,
             endOfMessage: true,
             cancellationToken);
    }

    private async Task<Response> DispatchAsync(Message message)
    {
        switch (message.Command)
        {
            case Command.Connect:
                {
                    return Connect(message.Payload.Deserialize<ConnectPayload>(_jsonSerializerOptions));
                }
            case Command.Query:
                {
                    return await QueryAsync(message.Payload.Deserialize<QueryPayload>(_jsonSerializerOptions));
                }
            default:
                throw new NotImplementedException();
        }
    }

    private async Task<Response> QueryAsync(QueryPayload queryPayload)
    {
        if (_databaseAdapter is null)
        {
            throw new NotSupportedException("Attempting to query before connect.");
        }
        var results = await _databaseAdapter.QueryAsync(queryPayload);
        return new Response(Command.QueryReturn, results);
    }

    private Response Connect(ConnectPayload connectPayload)
    {
        _databaseAdapter = _databaseAdapterFactory.GetDatabaseAdapter(connectPayload.DatabaseType);
        _databaseAdapter.Connect(connectPayload);
        return new Response(Command.ConnectReturn);
    }

    public void Dispose()
    {
        _databaseAdapter?.Dispose();
    }
}
