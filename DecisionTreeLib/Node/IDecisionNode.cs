using DecisionTreeLib.Enums;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public interface IDecisionNode<T> : INode<T>
{
    string Title { get; set; }
    string OperandKey { get; }
    T CompareValue { get; }
    RelationType RelationType { get; }
    INode<T> YesNextNode { get; set; }
    INode<T> NoNextNode { get; set; }
    Dictionary<Guid, IResponse<T>> ResultMap { get; set; }
}