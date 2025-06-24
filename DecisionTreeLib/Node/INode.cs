
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public interface INode<T> {
    Guid NodeId { get; set; }
    string Title { get; set; }
    Dictionary<Type, object> TypedResultMaps { get; }
    void AddResult<TData>(Guid nodeId, IResponse<TData> response);
    bool TryGetResult<TData>(Guid nodeId, out IResponse<TData>? response);
}