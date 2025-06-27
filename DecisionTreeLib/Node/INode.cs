using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public interface INode<TLeft, TRight, TResult>
{
    Guid NodeId { get; }
    string Title { get; }
    Dictionary<Guid, IResponse<TResult>> ResultMap { get; set; }
}