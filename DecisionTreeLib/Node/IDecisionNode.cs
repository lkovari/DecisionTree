using DecisionTreeLib.Request;

namespace DecisionTreeLib.Node;

public interface IDecisionNode<TLeft, TRight, TResult> : INode<TLeft, TRight, TResult>
{
    IDecisionRequest<TLeft, TRight> Request { get; }
    
    INode<TLeft, TRight, TResult> YesNextNode { get; }
    INode<TLeft, TRight, TResult> NoNextNode { get; }
}