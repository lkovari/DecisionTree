
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public interface INode<T> {
    Guid NodeId { get; set; }
    string Title { get; set; }
}