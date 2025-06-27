using DecisionTreeLib.Request;

namespace DecisionTreeLib.Node;

public interface IProcessNode<TLeft, TRight, TResult> : INode<TLeft, TRight, TResult>
{
    IOperationRequest<TLeft, TRight> Request { get; }
    INode<TLeft, TRight, TResult> NextNode { get; }
}