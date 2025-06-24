using DecisionTreeLib.Enums;
using DecisionTreeLib.Relation;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public class DecisionNode<T> : INode<T>
{
    public Guid NodeId { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string OperandKey { get; }
    public T CompareValue { get; }
    public RelationType RelationType { get; }
    public INode<T> YesNextNode { get; set; }
    public INode<T> NoNextNode { get; set; }
    public Dictionary<Guid, IResponse<T>> ResultMap { get; set; } = new();

    public DecisionNode(string title, string operandKey, T compareValue, RelationType relationType)
    {
        Title = title;
        OperandKey = operandKey;
        CompareValue = compareValue;
        RelationType = relationType;
    }
}