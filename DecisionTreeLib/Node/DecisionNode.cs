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
}