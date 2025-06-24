using DecisionTreeLib.Request;
using DecisionTreeLib.Response;
using DecisionTreeLib.Result;

namespace DecisionTreeLib.Node;

public interface INode<T> {
    Guid NodeId { get; set; }
    string Title { get; set; }
    Dictionary<Guid, IResponse<T>> ResultMap { get; set; }
}