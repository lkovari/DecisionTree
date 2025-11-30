using DecisionTreeLib.Adapters;
using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Evaluator;
using DecisionTreeLib.Node;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;
using DecisionTreeLibApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DecisionTreeLibApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DecisionTreeController : ControllerBase
{
    private readonly ILogger<DecisionTreeController> _logger;

    public DecisionTreeController(ILogger<DecisionTreeController> logger)
    {
        _logger = logger;
    }

    [HttpPost("evaluate/simple")]
    [ProducesResponseType(typeof(EvaluationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult EvaluateSimple([FromBody] SimpleEvaluationRequest request)
    {
        try
        {
            var leftData = new Data<int>(request.LeftValue);
            var rightData = new Data<int>(request.RightValue);

            INode<int, int, int> endNode;
            INode<int, int, int> calculationNode;

            if (!string.IsNullOrEmpty(request.Relation) && request.ExpectedValue.HasValue)
            {
                var relation = Enum.Parse<RelationType>(request.Relation);
                var expectedData = new Data<int>(request.ExpectedValue.Value);
                var decisionRequest = new DecisionRequest<int, int>(expectedData, new Data<int>(0), relation);
                
                var yesEndNode = new EndNode<int, int, int>(
                    "Yes Result",
                    new Response<int> { Title = "Yes Result", Result = new DecisionTreeLib.Result.Result<int> { Value = 100 } }
                );
                var noEndNode = new EndNode<int, int, int>(
                    "No Result",
                    new Response<int> { Title = "No Result", Result = new DecisionTreeLib.Result.Result<int> { Value = 0 } }
                );

                var decisionNode = new DecisionNode<int, int, int>(
                    $"Decision: {request.LeftValue} {request.Relation} {request.ExpectedValue}",
                    decisionRequest,
                    yesEndNode,
                    noEndNode
                );

                var operatorType = Enum.Parse<OperatorType>(request.Operation);
                var operationRequest = new BinaryOperationRequest<int, int>
                {
                    LeftOperand = leftData,
                    RightOperand = rightData,
                    Operator = operatorType
                };

                calculationNode = new CalculationNode<int, int, int>(
                    $"Calculation: {request.LeftValue} {request.Operation} {request.RightValue}",
                    operationRequest,
                    decisionNode
                );
            }
            else
            {
                var operatorType = Enum.Parse<OperatorType>(request.Operation);
                var operationRequest = new BinaryOperationRequest<int, int>
                {
                    LeftOperand = leftData,
                    RightOperand = rightData,
                    Operator = operatorType
                };

                endNode = new EndNode<int, int, int>(
                    "Result",
                    new Response<int> { Title = "Result", Result = new DecisionTreeLib.Result.Result<int> { Value = 0 } }
                );

                calculationNode = new CalculationNode<int, int, int>(
                    $"Calculation: {request.LeftValue} {request.Operation} {request.RightValue}",
                    operationRequest,
                    endNode
                );
            }

            var adapter = new StringAdapter();
            var evaluator = new DecisionTreeEvaluator(adapter);
            var result = evaluator.Evaluate(calculationNode);

            return Ok(new EvaluationResponse
            {
                Title = result.Title,
                Value = result.Result?.Value,
                Message = adapter.GetOutput()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error evaluating decision tree");
            return BadRequest(new EvaluationResponse
            {
                Message = $"Error: {ex.Message}"
            });
        }
    }

    [HttpPost("evaluate")]
    [ProducesResponseType(typeof(EvaluationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Evaluate([FromBody] EvaluationRequest request)
    {
        try
        {
            var adapter = new StringAdapter();
            var evaluator = new DecisionTreeEvaluator(adapter);
            
            var rootNode = BuildNode<int, int, int>(request.RootNode);
            var result = evaluator.Evaluate(rootNode);

            return Ok(new EvaluationResponse
            {
                Title = result.Title,
                Value = result.Result?.Value,
                Message = adapter.GetOutput()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error evaluating decision tree");
            return BadRequest(new EvaluationResponse
            {
                Message = $"Error: {ex.Message}"
            });
        }
    }

    [HttpGet("operators")]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
    public IActionResult GetOperators()
    {
        var operators = Enum.GetValues<OperatorType>()
            .ToDictionary(e => e.ToString(), e => e.ToString());
        
        return Ok(operators);
    }

    [HttpGet("relations")]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
    public IActionResult GetRelations()
    {
        var relations = Enum.GetValues<RelationType>()
            .ToDictionary(e => e.ToString(), e => e.ToString());
        
        return Ok(relations);
    }

    private INode<TLeft, TRight, TResult> BuildNode<TLeft, TRight, TResult>(NodeDto nodeDto)
    {
        return nodeDto.NodeType.ToLower() switch
        {
            "decision" => BuildDecisionNode<TLeft, TRight, TResult>(nodeDto),
            "calculation" => BuildCalculationNode<TLeft, TRight, TResult>(nodeDto),
            "unarycalculation" => BuildUnaryCalculationNode<TLeft, TRight, TResult>(nodeDto),
            "end" => BuildEndNode<TLeft, TRight, TResult>(nodeDto),
            _ => throw new ArgumentException($"Unknown node type: {nodeDto.NodeType}")
        };
    }

    private INode<TLeft, TRight, TResult> BuildDecisionNode<TLeft, TRight, TResult>(NodeDto nodeDto)
    {
        if (nodeDto.DecisionRequest == null)
            throw new ArgumentException("DecisionRequest is required for DecisionNode");

        var leftData = new Data<TLeft>((TLeft)Convert.ChangeType(nodeDto.DecisionRequest.LeftOperand, typeof(TLeft))!);
        var rightData = new Data<TRight>((TRight)Convert.ChangeType(nodeDto.DecisionRequest.RightOperand, typeof(TRight))!);
        var relation = Enum.Parse<RelationType>(nodeDto.DecisionRequest.Relation);

        var request = new DecisionRequest<TLeft, TRight>(leftData, rightData, relation);

        if (nodeDto.YesNextNode == null || nodeDto.NoNextNode == null)
            throw new ArgumentException("YesNextNode and NoNextNode are required for DecisionNode");

        var yesNode = BuildNode<TLeft, TRight, TResult>(nodeDto.YesNextNode);
        var noNode = BuildNode<TLeft, TRight, TResult>(nodeDto.NoNextNode);

        return new DecisionNode<TLeft, TRight, TResult>(nodeDto.Title, request, yesNode, noNode);
    }

    private INode<TLeft, TRight, TResult> BuildCalculationNode<TLeft, TRight, TResult>(NodeDto nodeDto)
    {
        if (nodeDto.BinaryOperationRequest == null)
            throw new ArgumentException("BinaryOperationRequest is required for CalculationNode");

        var leftData = new Data<TLeft>((TLeft)Convert.ChangeType(nodeDto.BinaryOperationRequest.LeftOperand, typeof(TLeft))!);
        var rightData = new Data<TRight>((TRight)Convert.ChangeType(nodeDto.BinaryOperationRequest.RightOperand, typeof(TRight))!);
        var operatorType = Enum.Parse<OperatorType>(nodeDto.BinaryOperationRequest.Operator);

        var request = new BinaryOperationRequest<TLeft, TRight>
        {
            LeftOperand = leftData,
            RightOperand = rightData,
            Operator = operatorType
        };

        if (nodeDto.NextNode == null)
            throw new ArgumentException("NextNode is required for CalculationNode");

        var nextNode = BuildNode<TLeft, TRight, TResult>(nodeDto.NextNode);

        return new CalculationNode<TLeft, TRight, TResult>(nodeDto.Title, request, nextNode);
    }

    private INode<TLeft, TRight, TResult> BuildUnaryCalculationNode<TLeft, TRight, TResult>(NodeDto nodeDto)
    {
        if (nodeDto.UnaryOperationRequest == null)
            throw new ArgumentException("UnaryOperationRequest is required for UnaryCalculationNode");

        var operandData = new Data<TLeft>((TLeft)Convert.ChangeType(nodeDto.UnaryOperationRequest.Operand, typeof(TLeft))!);
        var operatorType = Enum.Parse<OperatorType>(nodeDto.UnaryOperationRequest.Operator);

        var request = new UnaryOperationRequest<TLeft>
        {
            Operand = operandData,
            Operator = operatorType
        };

        if (nodeDto.NextNode == null)
            throw new ArgumentException("NextNode is required for UnaryCalculationNode");

        var nextNode = BuildNode<TLeft, object, TResult>(nodeDto.NextNode);

        var unaryNode = new UnaryCalculationNode<TLeft, TResult>(nodeDto.Title, request, nextNode);
        return (INode<TLeft, TRight, TResult>)unaryNode;
    }

    private INode<TLeft, TRight, TResult> BuildEndNode<TLeft, TRight, TResult>(NodeDto nodeDto)
    {
        if (nodeDto.EndNode == null)
            throw new ArgumentException("EndNode data is required for EndNode");

        var resultValue = (TResult)Convert.ChangeType(nodeDto.EndNode.ResultValue, typeof(TResult))!;
        var response = new Response<TResult>
        {
            Title = nodeDto.EndNode.Title,
            Result = new DecisionTreeLib.Result.Result<TResult> { Value = resultValue }
        };

        return new EndNode<TLeft, TRight, TResult>(nodeDto.Title, response);
    }
}

public class StringAdapter : IAdapter
{
    private readonly List<string> _messages = new();

    public void Write(string message)
    {
        _messages.Add(message);
    }

    public string GetOutput()
    {
        return string.Join("\n", _messages);
    }
}

