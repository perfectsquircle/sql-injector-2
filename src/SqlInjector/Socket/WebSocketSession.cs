using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SqlInjector.Socket;

public class WebSocketSession
{
    public static readonly JsonSerializerOptions JsonSerializerOptions;
    private readonly WebSocket _webSocket;
    private readonly IRouter _router;

    static WebSocketSession()
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public WebSocketSession(WebSocket webSocket, IRouter router)
    {
        _webSocket = webSocket;
        _router = router;
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
        var message = JsonSerializer.Deserialize<Request>(text, JsonSerializerOptions);

        Console.WriteLine(message);

        Response response;
        try
        {
            var payload = _router.Dispatch(message.Procedure, message.Payload);
            response = Response.FromResult(message.Id, payload);
        }
        catch (Exception e)
        {
            response = Response.FromError(message.Id, e.Message);
        }

        await _webSocket.SendAsync(
             Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response, JsonSerializerOptions)),
             WebSocketMessageType.Text,
             endOfMessage: true,
             cancellationToken);
    }
}
