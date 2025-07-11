using DecisionTreeLib.Request;

namespace DecisionTreeLib.Node;

public interface IUnaryCalculationNode<TOperand, TResult> : INode<TOperand, object, TResult>
{
    IUnaryRequest<TOperand> Request { get; }
    INode<TOperand, object, TResult> NextNode { get; }
} 