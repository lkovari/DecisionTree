using DecisionTreeLib.Request;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public class EndNode<T> : INode<T>
{
    public Dictionary<Guid, IResponse<T>> ResultMap { get; set; } = new();
    public Guid NodeId { get; set; }
    public string Title { get; set; }
    public Dictionary<Type, object> TypedResultMaps { get; } = new();

    public EndNode(string title)
    {
        Title = title;
        NodeId = Guid.NewGuid();
    }

    public IResponse<T> Process(IRequest<T> request)
    {
        Console.WriteLine("End of Processing.");
        return  new Response<T>
        {
            Title = "EndNode",
            Result = null
        };
    }

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