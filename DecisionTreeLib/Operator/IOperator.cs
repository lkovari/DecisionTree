using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Operator;

public interface IOperator
{
    OperatorType Operator { get; set; }
}