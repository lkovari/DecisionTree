using DecisionTreeLib.Enums;
using DecisionTreeLib.Operator;

namespace DecisionTreeLib.Node;

public interface IProcessNode<T> : INode<T>, IOperator {
    string LeftOperandKey { get; }
    string RightOperandKey { get; }
    OperatorType Operator { get; }
    INode<T> NextNode { get; set; }
}
