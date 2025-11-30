using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Evaluator;
using DecisionTreeLib.Node;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;
using DecisionTreeLib.Adapters;

namespace DecisionTreeLibTests;

public class UnaryCalculationNodeTests
{
    private class MockAdapter : IAdapter
    {
        public List<string> Messages { get; } = new();
        public void Write(string message) => Messages.Add(message);
    }

    [Fact]
    public void UnaryCalculationNode_Constructor_SetsProperties()
    {
        var request = new UnaryOperationRequest<int>
        {
            Operand = new Data<int>(5),
            Operator = OperatorType.Not
        };
        var nextNode = CreateEndNode("Next");
        var node = new UnaryCalculationNode<int, int>("Unary", request, nextNode);

        Assert.Equal("Unary", node.Title);
        Assert.Equal(request, node.Request);
        Assert.Equal(nextNode, node.NextNode);
        Assert.NotEqual(Guid.Empty, node.NodeId);
    }

    [Fact]
    public void UnaryCalculationNode_Execute_ReturnsOperandValue()
    {
        var request = new UnaryOperationRequest<int>
        {
            Operand = new Data<int>(42),
            Operator = OperatorType.Not
        };
        var nextNode = CreateEndNode("Next");
        var node = new UnaryCalculationNode<int, int>("Unary", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(42, Convert.ToInt32(result.Result.Value));
    }

    [Fact]
    public void UnaryCalculationNode_Execute_StoresResultInResultMap()
    {
        var request = new UnaryOperationRequest<int>
        {
            Operand = new Data<int>(5),
            Operator = OperatorType.Not
        };
        var nextNode = CreateEndNode("Next");
        var node = new UnaryCalculationNode<int, int>("Unary", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        node.Execute(evaluator);

        Assert.True(node.ResultMap.ContainsKey(node.NodeId));
        Assert.NotNull(node.ResultMap[node.NodeId]);
    }

    [Fact]
    public void UnaryCalculationNode_Execute_WritesToAdapter()
    {
        var request = new UnaryOperationRequest<int>
        {
            Operand = new Data<int>(5),
            Operator = OperatorType.Not
        };
        var nextNode = CreateEndNode("Next");
        var node = new UnaryCalculationNode<int, int>("Unary", request, nextNode);
        var adapter = new MockAdapter();
        var evaluator = new DecisionTreeEvaluator(adapter);

        node.Execute(evaluator);

        Assert.NotEmpty(adapter.Messages);
        Assert.Contains("Evaluator unary operation", adapter.Messages.First());
    }

    [Fact]
    public void UnaryCalculationNode_Execute_WithFloatOperand_ReturnsOperandValue()
    {
        var request = new UnaryOperationRequest<float>
        {
            Operand = new Data<float>(3.14f),
            Operator = OperatorType.Not
        };
        var nextNode = CreateEndNodeForFloat("Next");
        var node = new UnaryCalculationNode<float, float>("Unary", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal(3.14f, Convert.ToSingle(result.Result.Value), 0.001f);
    }

    [Fact]
    public void UnaryCalculationNode_Execute_WithStringOperand_ReturnsOperandValue()
    {
        var request = new UnaryOperationRequest<string>
        {
            Operand = new Data<string>("test"),
            Operator = OperatorType.Not
        };
        var nextNode = CreateEndNodeForString("Next");
        var node = new UnaryCalculationNode<string, string>("Unary", request, nextNode);
        var evaluator = new DecisionTreeEvaluator(null);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.Equal("test", result.Result.Value);
    }

    private static EndNode<int, object, int> CreateEndNode(string title)
    {
        return new EndNode<int, object, int>(title, new Response<int> { Title = title });
    }

    private static EndNode<float, object, float> CreateEndNodeForFloat(string title)
    {
        return new EndNode<float, object, float>(title, new Response<float> { Title = title });
    }

    private static EndNode<string, object, string> CreateEndNodeForString(string title)
    {
        return new EndNode<string, object, string>(title, new Response<string> { Title = title });
    }
}

