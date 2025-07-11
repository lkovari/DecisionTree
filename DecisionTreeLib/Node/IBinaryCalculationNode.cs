using DecisionTreeLib.Request;

namespace DecisionTreeLib.Node;

public interface IBinaryCalculationNode<TLeft, TRight, TResult> : INode<TLeft, TRight, TResult>
{
    IBinaryRequest<TLeft, TRight> Request { get; }
    INode<TLeft, TRight, TResult> NextNode { get; }
} 