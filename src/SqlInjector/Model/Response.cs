namespace SqlInjector.Model;

public class Response
{
    public Command Command { get; set; }
    public object Payload { get; set; }

    public Response(Command command)
    {
        Command = command;
    }

    public Response(Command command, object payload)
    {
        Command = command;
        Payload = payload;
    }

}
