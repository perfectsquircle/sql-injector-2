using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SqlInjector.Socket;

public interface IRouter
{
    object Dispatch(string procedure, JsonObject payload);
}

public abstract class Router : IRouter
{
    public object Dispatch(string procedure, JsonObject payload)
    {
        var method = GetType()
            .GetRuntimeMethods()
            .Where(m => m.GetCustomAttribute<ProcedureAttribute>() is not null
                && string.Equals(m.Name, procedure, StringComparison.OrdinalIgnoreCase))
            .SingleOrDefault();
        if (method is null)
        {
            throw new NotImplementedException($"{procedure} is not implemented.");
        }

        var parameterType = method.GetParameters().SingleOrDefault().ParameterType;
        var parameter = payload.Deserialize(parameterType, WebSocketSession.JsonSerializerOptions);

        try
        {
            return method.Invoke(this, new[] { parameter });
        }
        catch (TargetInvocationException e)
        {
            throw e.InnerException;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ProcedureAttribute : Attribute
    {
    }
}

