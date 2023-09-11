namespace SqlInjector.Socket;

public class Response
{
    public string Id { get; private set; }
    public string Error { get; private set; }
    public object Payload { get; private set; }

    public static Response FromResult(string id, object payload)
    {
        return new Response
        {
            Id = id,
            Payload = payload,
        };
    }

    public static Response FromError(string id, string error)
    {
        return new Response
        {
            Id = id,
            Error = error,
        };
    }
}
