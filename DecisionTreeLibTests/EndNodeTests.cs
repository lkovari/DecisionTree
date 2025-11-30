using DecisionTreeLib.Evaluator;
using DecisionTreeLib.Node;
using DecisionTreeLib.Response;
using DecisionTreeLib.Adapters;

namespace DecisionTreeLibTests;

public class EndNodeTests
{
    private class MockAdapter : IAdapter
    {
        public List<string> Messages { get; } = new();
        public void Write(string message) => Messages.Add(message);
    }

    [Fact]
    public void EndNode_Constructor_SetsProperties()
    {
        var result = new Response<int> { Title = "Result", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };
        var node = new EndNode<int, int, int>("End", result);

        Assert.Equal("End", node.Title);
        Assert.Equal(result, node.Result);
        Assert.NotEqual(Guid.Empty, node.NodeId);
    }

    [Fact]
    public void EndNode_Execute_ReturnsNodeResult_WhenNoParentResult()
    {
        var result = new Response<int> { Title = "Result", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };
        var node = new EndNode<int, int, int>("End", result);
        var evaluator = new DecisionTreeEvaluator(null);

        var response = node.Execute(evaluator);

        Assert.NotNull(response);
        Assert.Equal("End", response.Title);
        Assert.NotNull(response.Result);
        Assert.Equal(42, response.Result.Value);
    }

    [Fact]
    public void EndNode_Execute_ReturnsParentResult_WhenParentResultProvided()
    {
        var nodeResult = new Response<int> { Title = "NodeResult", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };
        var parentResult = new Response<int> { Title = "ParentResult", Result = new DecisionTreeLib.Result.Result<int> { Value = 100 } };
        var node = new EndNode<int, int, int>("End", nodeResult);
        var evaluator = new DecisionTreeEvaluator(null);

        var response = node.Execute(evaluator, parentResult);

        Assert.NotNull(response);
        Assert.Equal("End", response.Title);
        Assert.NotNull(response.Result);
        Assert.Equal(100, response.Result.Value);
    }

    [Fact]
    public void EndNode_Execute_StoresResultInResultMap()
    {
        var result = new Response<int> { Title = "Result", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };
        var node = new EndNode<int, int, int>("End", result);
        var evaluator = new DecisionTreeEvaluator(null);

        node.Execute(evaluator);

        Assert.True(node.ResultMap.ContainsKey(node.NodeId));
        Assert.NotNull(node.ResultMap[node.NodeId]);
    }

    [Fact]
    public void EndNode_Execute_WritesToAdapter()
    {
        var result = new Response<int> { Title = "Result", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };
        var node = new EndNode<int, int, int>("End", result);
        var adapter = new MockAdapter();
        var evaluator = new DecisionTreeEvaluator(adapter);

        node.Execute(evaluator);

        Assert.NotEmpty(adapter.Messages);
        Assert.Contains("Reached End Node", adapter.Messages.First());
    }

    [Fact]
    public void EndNode_Execute_WithBoolResult_ReturnsCorrectValue()
    {
        var result = new Response<bool> { Title = "Result", Result = new DecisionTreeLib.Result.Result<bool> { Value = true } };
        var node = new EndNode<int, int, bool>("End", result);
        var evaluator = new DecisionTreeEvaluator(null);

        var response = node.Execute(evaluator);

        Assert.NotNull(response);
        Assert.True(response.Result!.Value);
    }

    [Fact]
    public void EndNode_Execute_WithStringResult_ReturnsCorrectValue()
    {
        var result = new Response<string> { Title = "Result", Result = new DecisionTreeLib.Result.Result<string> { Value = "test" } };
        var node = new EndNode<int, int, string>("End", result);
        var evaluator = new DecisionTreeEvaluator(null);

        var response = node.Execute(evaluator);

        Assert.NotNull(response);
        Assert.Equal("test", response.Result!.Value);
    }
}

