using DecisionTreeLib.Data;

namespace DecisionTreeLib.Request;

public interface IRequest<T>
{
    Dictionary<string, IData<T>> Operands { get; }
}