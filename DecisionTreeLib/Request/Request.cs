using DecisionTreeLib.Data;

namespace DecisionTreeLib.Request;

public class Request<T> : IRequest<T>
{
    public Dictionary<string, IData<T>> Operands { get; } = new();
}