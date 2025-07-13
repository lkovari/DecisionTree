using DecisionTreeLib.Request;
using DecisionTreeLib.Response;
using DecisionTreeLib.Evaluator;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Helper;
using DecisionTreeLib.Validators;
using DecisionTreeLib.Extensions;

namespace DecisionTreeLib.Node;

public class CalculationNode<TLeft, TRight, TResult> : IBinaryCalculationNode<TLeft, TRight, TResult>
{
    public Guid NodeId { get; } = Guid.NewGuid();
    public string Title { get; }
    public Dictionary<Guid, IResponse<TResult>> ResultMap { get; set; } = new();

    public IBinaryRequest<TLeft, TRight> Request { get; }
    public INode<TLeft, TRight, TResult> NextNode { get; }

    public CalculationNode(string title, IBinaryRequest<TLeft, TRight> request, INode<TLeft, TRight, TResult> nextNode)
    {
        Title = title;
        Request = request;
        NextNode = nextNode;
    }

    public IResponse<TResult> Execute(DecisionTreeEvaluator evaluator, IResponse<TResult>? parentResult = null)
    {
        if (IsLogicalOperation(Request.Operator))
        {
            OperandTypeValidator.ValidateBitwiseOperands(Request.LeftOperand, Request.RightOperand);
        }
        else
        {
            OperandTypeValidator.ValidateArithmeticOperands(Request.LeftOperand, Request.RightOperand);
        }
        
        var left = Request.LeftOperand.Value;
        var right = Request.RightOperand.Value;
        var result = CalculateOperation(left, right, Request.Operator);
        
        var response = new Response<TResult>
        {
            Title = Title,
            Result = new DecisionTreeLib.Result.Result<TResult> { Value = (TResult)Convert.ChangeType(result, typeof(TResult))! }
        };
        
        ResultMap[NodeId] = response;
        
        var mess = ExpressionTextFormatHelper.FormatOperation(
            left, 
            Request.Operator.ToSymbol(), 
            right, 
            result);
        
        evaluator.WriteToAdapter($" Evaluator operation: {mess}");
        
        return evaluator.Evaluate(NextNode, response);
    }

    // WARNING: Using dynamic disables compile-time type safety. Only use for numeric types.
    private object CalculateOperation(object left, object right, OperatorType operationType)
    {
        dynamic l = left;
        dynamic r = right;

        return operationType switch
        {
            OperatorType.Add => l + r,
            OperatorType.Subtract => l - r,
            OperatorType.Multiply => l * r,
            OperatorType.Divide => r != 0 ? l / r : throw new DivideByZeroException(),
            OperatorType.And => l & r,
            OperatorType.Or => l | r,
            OperatorType.Xor => l ^ r,
            OperatorType.Not => ~l,
            OperatorType.Nand => ~(l & r),
            OperatorType.Nor => ~(l | r),
            _ => throw new InvalidOperationException($"Unsupported OperatorType: {operationType}")
        };
    }

    private static bool IsLogicalOperation(OperatorType operationType) =>
        operationType switch
        {
            OperatorType.And => true,
            OperatorType.Or => true,
            OperatorType.Xor => true,
            OperatorType.Not => true,
            OperatorType.Nand => true,
            OperatorType.Nor => true,
            _ => false
        };
}