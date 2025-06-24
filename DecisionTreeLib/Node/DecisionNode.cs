using DecisionTreeLib.Enums;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public class DecisionNode<T>(
    string title,
    string operandKey,
    T compareValue,
    RelationType relationType,
    INode<T>? yesNextNode,
    INode<T>? noNextNode)
    : IDecisionNode<T>
{
    public Guid NodeId { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = title;
    public string OperandKey { get; } = operandKey;
    public T CompareValue { get; } = compareValue;
    public RelationType RelationType { get; } = relationType;
    public INode<T>? YesNextNode { get; set; } = yesNextNode;
    public INode<T>? NoNextNode { get; set; } = noNextNode;
    public Dictionary<Guid, IResponse<T>> ResultMap { get; set; }
    public Dictionary<Type, object> TypedResultMaps { get; } = new();
    public void AddResult<TData>(Guid nodeId, IResponse<TData> response)
    {
        if (!TypedResultMaps.TryGetValue(typeof(TData), out var mapObj))
        {
            mapObj = new Dictionary<Guid, IResponse<TData>>();
            TypedResultMaps[typeof(TData)] = mapObj;
        }

        var map = (Dictionary<Guid, IResponse<TData>>)mapObj;
        map[nodeId] = response;
    }

    public bool TryGetResult<TData>(Guid nodeId, out IResponse<TData>? response)
    {
        response = default;

        if (TypedResultMaps.TryGetValue(typeof(TData), out var mapObj))
        {
            var map = (Dictionary<Guid, IResponse<TData>>)mapObj;
            return map.TryGetValue(nodeId, out response);
        }

        return false;
    }
}