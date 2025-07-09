namespace DecisionTreeLib.Node;

using DecisionTreeLib.Response;
using DecisionTreeLib.Evaluator;

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

    public IResponse<TResult> Execute(DecisionTreeEvaluator evaluator, IResponse<TResult>? parentResult = null)
    {
        evaluator.WriteToAdapter($" Reached End Node: {Title}");
        
        var finalResponse = new Response<TResult>
        {
            Title = Title,
            Result = parentResult?.Result ?? Result.Result
        };

        ResultMap[NodeId] = finalResponse;

        return finalResponse;
    }
}