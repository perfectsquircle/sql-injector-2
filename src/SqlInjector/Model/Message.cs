using System.Text.Json.Nodes;

namespace SqlInjector.Model;

public record class Message
{
    public Command Command { get; set; }
    public JsonObject Payload { get; set; }
}
