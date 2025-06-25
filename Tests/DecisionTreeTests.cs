using DecisionTreeLib.Adapters;
using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Helper;
using DecisionTreeLib.Node;
using DecisionTreeLib.Processing;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;
using DecisionTreeLib.Result;
using Xunit;

namespace Tests;

public class ProcessAndDecisionNodeTests
{
    [Theory]
    [InlineData(5, 3, OperatorType.Add, 8)]
    [InlineData(5, 3, OperatorType.Subtract, 2)]
    [InlineData(5, 3, OperatorType.Multiply, 15)]
    [InlineData(6, 3, OperatorType.Divide, 2)]
    public void ProcessNode_Should_Perform_Arithmetic_Correctly(double left, double right, OperatorType op, double expected)
    {
        ResponseStorageHelper.ClearAll();
        var adapter = new ConsoleAdapter<double>();
        var request = new Request<double>
        {
            Operands =
            {
                ["left"] = new Data<double>(left),
                ["right"] = new Data<double>(right)
            }
        };

        var endNode = new EndNode<double>();
        var processNode = new ProcessNode<double>("Result", "left", "right", op, endNode);

        var processor = new Processor<double>(adapter);
        
        processor.Process(processNode, request);
        
        Assert.True(request.Operands.ContainsKey("Result"));
        Assert.Equal(expected, request.Operands["Result"].Value);
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
        ResponseStorageHelper.ClearAll();

        var request = new Request<double>
        {
            Operands = { ["left"] = new Data<double>(left) }
        };

        var yesEndNode = new EndNode<double> { Title = "YesEnd" };
        var noEndNode = new EndNode<double> { Title = "NoEnd" };

        var decisionNode = new DecisionNode<double>(
            title: "Decision",
            operandKey: "left",
            compareValue: right,
            relationType: relation,
            yesNextNode: yesEndNode,
            noNextNode: noEndNode
        );

        var processor = new Processor<double>(new ConsoleAdapter<double>());
        
        processor.Process(decisionNode, request);
        
        bool yesExecuted = ResponseStorageHelper.GetResultMap<string>()?.ContainsKey(yesEndNode.NodeId) == true;
        bool noExecuted = ResponseStorageHelper.GetResultMap<string>()?.ContainsKey(noEndNode.NodeId) == true;

        Assert.Equal(expectedYes, yesExecuted);
        Assert.Equal(!expectedYes, noExecuted);
    }
}
