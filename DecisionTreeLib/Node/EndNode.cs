namespace DecisionTreeLib.Node;

using DecisionTreeLib.Response;

public class EndNode<TLeft, TRight, TResult> : INode<TLeft, TRight, TResult>
{
    public Guid NodeId { get; } = Guid.NewGuid();
    public string Title { get; }
    public Dictionary<Guid, IResponse<TResult>> ResultMap { get; set; } = new();

    public IResponse<TResult> Result { get; }

    public EndNode(string title, IResponse<TResult> result)
    {
        Title = title;
        Result = result;
    }
}