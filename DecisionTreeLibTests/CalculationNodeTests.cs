using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Evaluator;
using DecisionTreeLib.Node;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;
using DecisionTreeLib.Adapters;

namespace DecisionTreeLibTests;

public class CalculationNodeTests
{
    private class MockAdapter : IAdapter
    {
        public List<string> Messages { get; } = new();
        public void Write(string message) => Messages.Add(message);
    }

    [Fact]
    public void CalculationNode_Constructor_SetsProperties()
    {
        var request = new BinaryOperationRequest<int, int>
        {
            LeftOperand = new Data<int>(5),
            RightOperand = new Data<int>(3),
            Operator = OperatorType.Add
        };
        var nextNode = CreateEndNode("Next");
        var node = new CalculationNode<int, int, int>("Calculation", request, nextNode);

        Assert.Equal("Calculation", node.Title);
        Assert.Equal(request, node.Request);
        Assert.Equal(nextNode, node.NextNode);
        Assert.NotEqual(Guid.Empty, node.NodeId);
    }

    [Theory]
    [InlineData(5, 3, OperatorType.Add, 8)]
    [InlineData(10, 4, OperatorType.Subtract, 6)]
    [InlineData(5, 3, OperatorType.Multiply, 15)]
    [InlineData(10, 2, OperatorType.Divide, 5)]
    public void CalculationNode_Execute_WithArithmeticOperations_ReturnsCorrectResult(int left, int right, OperatorType op, int expected)
    {
        var request = new BinaryOperationRequest<int, int>
        {
            LeftOperand = new Data<int>(left),
            RightOperand = new Data<int>(right),
            Operator = op
        };
        var nextNode = CreateEndNode("Next");
        var node = new CalculationNode<int, int, int>("Calculation", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(expected, Convert.ToInt32(result.Result.Value));
    }

    [Theory]
    [InlineData(5, 3, OperatorType.And, 5 & 3)]
    [InlineData(5, 3, OperatorType.Or, 5 | 3)]
    [InlineData(5, 3, OperatorType.Xor, 5 ^ 3)]
    [InlineData(5, 3, OperatorType.Nand, ~(5 & 3))]
    [InlineData(5, 3, OperatorType.Nor, ~(5 | 3))]
    public void CalculationNode_Execute_WithBitwiseOperations_ReturnsCorrectResult(int left, int right, OperatorType op, int expected)
    {
        var request = new BinaryOperationRequest<int, int>
        {
            LeftOperand = new Data<int>(left),
            RightOperand = new Data<int>(right),
            Operator = op
        };
        var nextNode = CreateEndNode("Next");
        var node = new CalculationNode<int, int, int>("Calculation", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(expected, Convert.ToInt32(result.Result.Value));
    }

    [Fact]
    public void CalculationNode_Execute_WithDivideByZero_ThrowsDivideByZeroException()
    {
        var request = new BinaryOperationRequest<int, int>
        {
            LeftOperand = new Data<int>(10),
            RightOperand = new Data<int>(0),
            Operator = OperatorType.Divide
        };
        var nextNode = CreateEndNode("Next");
        var node = new CalculationNode<int, int, int>("Calculation", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        Assert.Throws<DivideByZeroException>(() => node.Execute(evaluator));
    }

    [Fact]
    public void CalculationNode_Execute_WithFloatArithmetic_ReturnsCorrectResult()
    {
        var request = new BinaryOperationRequest<float, float>
        {
            LeftOperand = new Data<float>(5.5f),
            RightOperand = new Data<float>(2.5f),
            Operator = OperatorType.Add
        };
        var nextNode = CreateEndNodeForFloat("Next");
        var node = new CalculationNode<float, float, float>("Calculation", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(8.0f, Convert.ToSingle(result.Result.Value), 0.001f);
    }

    [Fact]
    public void CalculationNode_Execute_WithInvalidOperandTypes_ThrowsInvalidOperandTypeException()
    {
        var request = new BinaryOperationRequest<string, string>
        {
            LeftOperand = new Data<string>("5"),
            RightOperand = new Data<string>("3"),
            Operator = OperatorType.Add
        };
        var nextNode = CreateEndNodeForString("Next");
        var node = new CalculationNode<string, string, string>("Calculation", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        Assert.Throws<DecisionTreeLib.Validators.InvalidOperandTypeException>(() => node.Execute(evaluator));
    }

    [Fact]
    public void CalculationNode_Execute_WithBitwiseOperationOnNonInteger_ThrowsInvalidOperandTypeException()
    {
        var request = new BinaryOperationRequest<float, float>
        {
            LeftOperand = new Data<float>(5.5f),
            RightOperand = new Data<float>(3.3f),
            Operator = OperatorType.And
        };
        var nextNode = CreateEndNodeForFloat("Next");
        var node = new CalculationNode<float, float, float>("Calculation", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        Assert.Throws<DecisionTreeLib.Validators.InvalidOperandTypeException>(() => node.Execute(evaluator));
    }

    [Fact]
    public void CalculationNode_Execute_StoresResultInResultMap()
    {
        var request = new BinaryOperationRequest<int, int>
        {
            LeftOperand = new Data<int>(5),
            RightOperand = new Data<int>(3),
            Operator = OperatorType.Add
        };
        var nextNode = CreateEndNode("Next");
        var node = new CalculationNode<int, int, int>("Calculation", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        node.Execute(evaluator);

        Assert.True(node.ResultMap.ContainsKey(node.NodeId));
        Assert.NotNull(node.ResultMap[node.NodeId]);
    }

    [Fact]
    public void CalculationNode_Execute_WritesToAdapter()
    {
        var request = new BinaryOperationRequest<int, int>
        {
            LeftOperand = new Data<int>(5),
            RightOperand = new Data<int>(3),
            Operator = OperatorType.Add
        };
        var nextNode = CreateEndNode("Next");
        var node = new CalculationNode<int, int, int>("Calculation", request, nextNode);
        var adapter = new MockAdapter();
        var evaluator = new DecisionTreeEvaluator(adapter);

        node.Execute(evaluator);

        Assert.NotEmpty(adapter.Messages);
        Assert.Contains("Evaluator operation", adapter.Messages.First());
    }

    [Fact]
    public void CalculationNode_Execute_WithDecimalArithmetic_ReturnsCorrectResult()
    {
        var request = new BinaryOperationRequest<decimal, decimal>
        {
            LeftOperand = new Data<decimal>(10.5m),
            RightOperand = new Data<decimal>(2.5m),
            Operator = OperatorType.Multiply
        };
        var nextNode = CreateEndNodeForDecimal("Next");
        var node = new CalculationNode<decimal, decimal, decimal>("Calculation", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(26.25m, Convert.ToDecimal(result.Result.Value));
    }

    private static EndNode<int, int, int> CreateEndNode(string title)
    {
        return new EndNode<int, int, int>(title, new Response<int> { Title = title });
    }

    private static EndNode<float, float, float> CreateEndNodeForFloat(string title)
    {
        return new EndNode<float, float, float>(title, new Response<float> { Title = title });
    }

    private static EndNode<string, string, string> CreateEndNodeForString(string title)
    {
        return new EndNode<string, string, string>(title, new Response<string> { Title = title });
    }

    private static EndNode<decimal, decimal, decimal> CreateEndNodeForDecimal(string title)
    {
        return new EndNode<decimal, decimal, decimal>(title, new Response<decimal> { Title = title });
    }
}

