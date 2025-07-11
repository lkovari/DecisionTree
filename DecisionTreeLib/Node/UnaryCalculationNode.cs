using DecisionTreeLib.Request;
using DecisionTreeLib.Response;
using DecisionTreeLib.Evaluator;

namespace DecisionTreeLib.Node;

public class UnaryCalculationNode<TOperand, TResult> : IUnaryCalculationNode<TOperand, TResult>
{
    public Guid NodeId { get; } = Guid.NewGuid();
    public string Title { get; }
    public Dictionary<Guid, IResponse<TResult>> ResultMap { get; set; } = new();
    public IUnaryRequest<TOperand> Request { get; }
    public INode<TOperand, object, TResult> NextNode { get; }

    public UnaryCalculationNode(string title, IUnaryRequest<TOperand> request, INode<TOperand, object, TResult> nextNode)
    {
        Title = title;
        Request = request;
        NextNode = nextNode;
    }

    public IResponse<TResult> Execute(DecisionTreeEvaluator evaluator, IResponse<TResult>? parentResult = null)
    {
        var operand = Request.Operand.Value;
        var result = operand;
        var response = new Response<TResult>
        {
            Title = Title,
            Result = new DecisionTreeLib.Result.Result<TResult> { Value = (TResult)Convert.ChangeType(result, typeof(TResult))! }
        };
        ResultMap[NodeId] = response;
        evaluator.WriteToAdapter($" Evaluator unary operation: {operand}");
        return evaluator.Evaluate(NextNode, response);
    }
} 