using DecisionTreeLib.Adapters;
using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Node;
using DecisionTreeLib.Processing;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;
using Xunit;

namespace Tests;

public class ProcessAndDecisionNodeTests
{
    private readonly IAdapter _adapter = new ConsoleAdapter();

    [Theory]
    [InlineData(5, 3, OperatorType.Add, 8)]
    [InlineData(5, 3, OperatorType.Subtract, 2)]
    [InlineData(5, 3, OperatorType.Multiply, 15)]
    [InlineData(6, 3, OperatorType.Divide, 2)]
    public void ProcessNode_Should_Perform_Arithmetic_Correctly(double left, double right, OperatorType op, double expected)
    {
        var request = new OperationRequest<double, double>(
            new Data<double>(left),
            new Data<double>(right),
            op
        );

        var endNode = new EndNode<double, double, double>("End", new Response<double> { Title = "End" });

        var processNode = new ProcessNode<double, double, double>("Process", request, endNode);

        var evaluator = new DecisionTreeEvaluator(_adapter);
        var response = evaluator.Evaluate(processNode);

        Assert.NotNull(response?.Result);
        Assert.Equal(expected, response.Result!.Value);
    }

    [Theory]
    [InlineData(5, 5, RelationType.Equal, true)]
    [InlineData(5, 3, RelationType.GreaterThan, true)]
    [InlineData(3, 5, RelationType.LessThan, true)]
    [InlineData(5, 3, RelationType.GreaterThanOrEqual, true)]
    [InlineData(3, 3, RelationType.GreaterThanOrEqual, true)]
    [InlineData(3, 5, RelationType.LessThanOrEqual, true)]
    [InlineData(3, 3, RelationType.LessThanOrEqual, true)]
    [InlineData(3, 5, RelationType.NotEqual, true)]
    public void DecisionNode_Should_Evaluate_Condition_Correctly(double left, double right, RelationType relation, bool expectedYes)
    {
        var request = new DecisionRequest<double, double>(
            new Data<double>(left),
            new Data<double>(right),
            relation
        );

        var yesEndNode = new EndNode<double, double, bool>("YesEnd", new Response<bool> { Title = "YesEnd" });
        var noEndNode = new EndNode<double, double, bool>("NoEnd", new Response<bool> { Title = "NoEnd" });

        var decisionNode = new DecisionNode<double, double, bool>(
            "Decision",
            request,
            yesEndNode,
            noEndNode
        );

        var evaluator = new DecisionTreeEvaluator(_adapter);
        var response = evaluator.Evaluate(decisionNode);

        Assert.NotNull(response);
        Assert.True(response.Result != null);

        if (expectedYes)
        {
            Assert.True(response.Result!.Value);
            Assert.Equal("YesEnd", response.Title);
        }
        else
        {
            Assert.False(response.Result!.Value);
            Assert.Equal("NoEnd", response.Title);
        }
    }
}