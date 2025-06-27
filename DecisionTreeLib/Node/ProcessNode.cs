using DecisionTreeLib.Request;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public class ProcessNode<TLeft, TRight, TResult> : IProcessNode<TLeft, TRight, TResult>
{
    public Guid NodeId { get; } = Guid.NewGuid();
    public string Title { get; }
    public Dictionary<Guid, IResponse<TResult>> ResultMap { get; set; } = new();

    public IOperationRequest<TLeft, TRight> Request { get; }
    public INode<TLeft, TRight, TResult> NextNode { get; }

    public ProcessNode(string title, IOperationRequest<TLeft, TRight> request, INode<TLeft, TRight, TResult> nextNode)
    {
        Title = title;
        Request = request;
        NextNode = nextNode;
    }
}