using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Operator;

public class Operator : IOperator
{
    private OperatorType Type { get; set; }

    OperatorType IOperator.Operator
    {
        get => Type;
        set => Type = value;
    }

    public Operator(OperatorType type)
    {
        Type = type;
    }
}