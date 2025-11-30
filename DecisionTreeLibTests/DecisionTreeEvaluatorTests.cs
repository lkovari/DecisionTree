using DecisionTreeLib.Adapters;
using DecisionTreeLib.Evaluator;
using DecisionTreeLib.Node;
using DecisionTreeLib.Response;

namespace DecisionTreeLibTests;

public class DecisionTreeEvaluatorTests
{
    private class MockAdapter : IAdapter
    {
        public List<string> Messages { get; } = new();
        public void Write(string message) => Messages.Add(message);
    }

    [Fact]
    public void DecisionTreeEvaluator_Constructor_WithAdapter_SetsAdapter()
    {
        var adapter = new MockAdapter();
        var evaluator = new DecisionTreeEvaluator(adapter);

        Assert.NotNull(evaluator);
    }

    [Fact]
    public void DecisionTreeEvaluator_Constructor_WithNullAdapter_DoesNotThrow()
    {
        var evaluator = new DecisionTreeEvaluator(null);

        Assert.NotNull(evaluator);
    }

    [Fact]
    public void DecisionTreeEvaluator_Evaluate_CallsNodeExecute()
    {
        var endNode = new EndNode<int, int, int>("End", new Response<int> { Title = "End", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } });
        var evaluator = new DecisionTreeEvaluator(null);

        var result = evaluator.Evaluate(endNode);

        Assert.NotNull(result);
        Assert.Equal(42, result.Result!.Value);
    }

    [Fact]
    public void DecisionTreeEvaluator_Evaluate_WritesToAdapter()
    {
        var adapter = new MockAdapter();
        var evaluator = new DecisionTreeEvaluator(adapter);
        var endNode = new EndNode<int, int, int>("End", new Response<int> { Title = "End", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } });

        evaluator.Evaluate(endNode);

        Assert.NotEmpty(adapter.Messages);
        Assert.Contains("Evaluating", adapter.Messages.First());
    }

    [Fact]
    public void DecisionTreeEvaluator_Evaluate_WithParentResult_PassesToNode()
    {
        var parentResult = new Response<int> { Title = "Parent", Result = new DecisionTreeLib.Result.Result<int> { Value = 100 } };
        var endNode = new EndNode<int, int, int>("End", new Response<int> { Title = "End", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } });
        var evaluator = new DecisionTreeEvaluator(null);

        var result = evaluator.Evaluate(endNode, parentResult);

        Assert.NotNull(result);
        Assert.Equal(100, result.Result!.Value);
    }

}

