using DecisionTreeLib.Enums;
using DecisionTreeLib.Node;
using DecisionTreeLib.Response;

public class ProcessNode<T> : IProcessNode<T>
{
    public Guid NodeId { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string LeftOperandKey { get; }
    public string RightOperandKey { get; }
    public OperatorType Operator { get; }
    public INode<T> NextNode { get; set; }
    public Dictionary<Guid, IResponse<T>> ResultMap { get; set; } = new();

    public ProcessNode(string title, string leftKey, string rightKey, OperatorType op)
    {
        Title = title;
        LeftOperandKey = leftKey;
        RightOperandKey = rightKey;
        Operator = op;
    }
}