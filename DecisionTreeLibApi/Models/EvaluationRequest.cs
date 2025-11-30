namespace DecisionTreeLibApi.Models;

public class EvaluationRequest
{
    public string Title { get; set; } = string.Empty;
    public NodeDto RootNode { get; set; } = null!;
}

public class NodeDto
{
    public string NodeType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DecisionRequestDto? DecisionRequest { get; set; }
    public BinaryOperationRequestDto? BinaryOperationRequest { get; set; }
    public UnaryOperationRequestDto? UnaryOperationRequest { get; set; }
    public EndNodeDto? EndNode { get; set; }
    public NodeDto? YesNextNode { get; set; }
    public NodeDto? NoNextNode { get; set; }
    public NodeDto? NextNode { get; set; }
}

public class DecisionRequestDto
{
    public object LeftOperand { get; set; } = null!;
    public object RightOperand { get; set; } = null!;
    public string Relation { get; set; } = string.Empty;
}

public class BinaryOperationRequestDto
{
    public object LeftOperand { get; set; } = null!;
    public object RightOperand { get; set; } = null!;
    public string Operator { get; set; } = string.Empty;
}

public class UnaryOperationRequestDto
{
    public object Operand { get; set; } = null!;
    public string Operator { get; set; } = string.Empty;
}

public class EndNodeDto
{
    public string Title { get; set; } = string.Empty;
    public object ResultValue { get; set; } = null!;
}

