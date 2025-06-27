using DecisionTreeLib.Request;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public class DecisionNode<TLeft, TRight, TResult> : IDecisionNode<TLeft, TRight, TResult>
{
    public Guid NodeId { get; } = Guid.NewGuid();
    public string Title { get; }
    public Dictionary<Guid, IResponse<TResult>> ResultMap { get; set; } = new();

    public IDecisionRequest<TLeft, TRight> Request { get; }
    public INode<TLeft, TRight, TResult> YesNextNode { get; }
    public INode<TLeft, TRight, TResult> NoNextNode { get; }

    public DecisionNode(string title, IDecisionRequest<TLeft, TRight> request, INode<TLeft, TRight, TResult> yesNextNode, INode<TLeft, TRight, TResult> noNextNode)
    {
        Title = title;
        Request = request;
        YesNextNode = yesNextNode;
        NoNextNode = noNextNode;
    }
}