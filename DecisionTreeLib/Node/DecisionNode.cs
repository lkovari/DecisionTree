using DecisionTreeLib.Request;
using DecisionTreeLib.Response;
using DecisionTreeLib.Evaluator;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Helper;

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

    public IResponse<TResult> Execute(DecisionTreeEvaluator evaluator, IResponse<TResult>? parentResult = null)
    {
        bool condition = Request.Relation switch
        {
            RelationType.LessThan => Comparator.Compare(Request.LeftOperand, Request.RightOperand) < 0,
            RelationType.LessThanOrEqual => Comparator.Compare(Request.LeftOperand, Request.RightOperand) <= 0,
            RelationType.GreaterThan => Comparator.Compare(Request.LeftOperand, Request.RightOperand) > 0,
            RelationType.GreaterThanOrEqual => Comparator.Compare(Request.LeftOperand, Request.RightOperand) >= 0,
            RelationType.Equal => Equals(Request.LeftOperand.Value, Request.RightOperand.Value),
            RelationType.NotEqual => !Equals(Request.LeftOperand.Value, Request.RightOperand.Value),
            RelationType.Contains => Request.LeftOperand?.ToString()?.Contains(Request.RightOperand?.ToString() ?? string.Empty) ?? false,
            _ => throw new InvalidOperationException($"Unsupported RelationType: {Request.Relation}")
        };

        var response = new Response<TResult>
        {
            Title = Title,
            Result = new DecisionTreeLib.Result.Result<TResult> { Value = (TResult)Convert.ChangeType(condition, typeof(TResult))! }
        };
        
        ResultMap[NodeId] = response;

        var mess = ExpressionTextFormatHelper.FormatRelation(
            Request.LeftOperand.Value, 
            Request.Relation.ToString(), 
            Request.RightOperand.Value, 
            condition);
        
        evaluator.WriteToAdapter($" Evaluating decision: {mess}");
        
        return evaluator.Evaluate(condition ? YesNextNode : NoNextNode, response);
    }
}