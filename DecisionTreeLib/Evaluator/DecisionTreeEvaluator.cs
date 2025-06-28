using DecisionTreeLib.Adapters;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Helper;
using DecisionTreeLib.Node;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Evaluator;

public class DecisionTreeEvaluator
{
    private readonly IAdapter? _adapter;

    public DecisionTreeEvaluator(IAdapter? adapter)
    {
        _adapter = adapter;
    }

    public IResponse<TResult> Evaluate<TLeft, TRight, TResult>(INode<TLeft, TRight, TResult> node, IResponse<TResult> parentResult = null)
    {
        _adapter?.Write($"Evaluating {node.Title}");
        switch (node)
        {
            case ICalculationNode<TLeft, TRight, TResult> CalculationNode:
                return EvaluateCalculationNode(CalculationNode);

            case IDecisionNode<TLeft, TRight, TResult> decisionNode:
                return EvaluateDecisionNode(decisionNode);

            case EndNode<TLeft, TRight, TResult> endNode:
                _adapter?.Write($" Reached End Node: {endNode.Title}");
                var finalResponse = new Response<TResult>
                {
                    Title = endNode.Title,
                    Result = parentResult.Result
                };

                endNode.ResultMap[endNode.NodeId] = finalResponse;

                return finalResponse;

            default:
                throw new InvalidOperationException($"Unsupported node type: {node.GetType().Name}");
        }
    }

    private IResponse<TResult> EvaluateCalculationNode<TLeft, TRight, TResult>(ICalculationNode<TLeft, TRight, TResult> node)
    {
        var left = node.Request.LeftOperand.Value;
        var right = node.Request.RightOperand.Value;
        var result = CalculateOperation(left, right, node.Request.Operator);
        
        var response = new Response<TResult>
        {
            Title = node.Title,
            Result = new DecisionTreeLib.Result.Result<TResult> { Value = (TResult)Convert.ChangeType(result, typeof(TResult))! }
        };
        
        node.ResultMap[node.NodeId] = response;
        
        var mess = ExpressionTextFormatHelper.FormatOperation(
            left, 
            "" + (char)node.Request.Operator, 
            right, 
            result);
        
        _adapter.Write($" Evaluator operation: {mess}");
        return Evaluate(node.NextNode, response);
    }

    private IResponse<TResult> EvaluateDecisionNode<TLeft, TRight, TResult>(IDecisionNode<TLeft, TRight, TResult> node)
    {
        bool condition = node.Request.Relation switch
        {
            RelationType.LessThan => Comparator.Compare(node.Request.LeftOperand, node.Request.RightOperand) < 0,
            RelationType.LessThanOrEqual => Comparator.Compare(node.Request.LeftOperand, node.Request.RightOperand) <= 0,
            RelationType.GreaterThan => Comparator.Compare(node.Request.LeftOperand, node.Request.RightOperand) > 0,
            RelationType.GreaterThanOrEqual => Comparator.Compare(node.Request.LeftOperand, node.Request.RightOperand) >= 0,
            RelationType.Equal => Equals(node.Request.LeftOperand.Value, node.Request.RightOperand.Value),
            RelationType.NotEqual => !Equals(node.Request.LeftOperand.Value, node.Request.RightOperand.Value),
            RelationType.Contains => node.Request.LeftOperand?.ToString()?.Contains(node.Request.RightOperand?.ToString() ?? string.Empty) ?? false,
            _ => throw new InvalidOperationException($"Unsupported RelationType: {node.Request.Relation}")
        };

        var response = new Response<TResult>
        {
            Title = node.Title,
            Result = new DecisionTreeLib.Result.Result<TResult> { Value = (TResult)Convert.ChangeType(condition, typeof(TResult))! }
        };
        
        node.ResultMap[node.NodeId] = response;

        var mess = ExpressionTextFormatHelper.FormatRelation(
            node.Request.LeftOperand.Value, 
            node.Request.Relation.ToString(), 
            node.Request.RightOperand.Value, 
            condition);
        
        _adapter?.Write($" Evaluating decision: {mess}");
        return Evaluate(condition ? node.YesNextNode : node.NoNextNode, response);
    }

    private object CalculateOperation<TLeft, TRight>(TLeft left, TRight right, OperatorType operationType)
    {
        dynamic l = left;
        dynamic r = right;

        return operationType switch
        {
            OperatorType.Add => l + r,
            OperatorType.Subtract => l - r,
            OperatorType.Multiply => l * r,
            OperatorType.Divide => r != 0 ? l / r : throw new DivideByZeroException(),
            _ => throw new InvalidOperationException($"Unsupported OperatorType: {operationType}")
        };
    }
}