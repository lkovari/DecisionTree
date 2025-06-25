using DecisionTreeLib.Helper;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public class EndNode<T> : INode<T>
{
    public Dictionary<Guid, IResponse<T>> ResultMap { get; set; } = new();
    public Guid NodeId { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
}