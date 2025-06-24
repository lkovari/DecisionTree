using DecisionTreeLib.Enums;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public class ProcessNode<T>(string title, string leftKey, string rightKey, OperatorType op, INode<T> nextNode)
    : IProcessNode<T>
{
    public Guid NodeId { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = title;
    public string LeftOperandKey { get; } = leftKey;
    public string RightOperandKey { get; } = rightKey;
    public OperatorType Operator { get; } = op;
    public INode<T> NextNode { get; set; } = nextNode;
    public Dictionary<Guid, IResponse<T>> ResultMap { get; set; } = new();
}