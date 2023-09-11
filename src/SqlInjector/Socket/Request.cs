using System.Text.Json.Nodes;

namespace SqlInjector.Socket;

public record class Request
{
    public string Id { get; set; }
    public string Procedure { get; set; }
    public JsonObject Payload { get; set; }
}
