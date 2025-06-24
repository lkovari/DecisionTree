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
    [Theory]
    [InlineData(5, 3, OperatorType.Add, 8)]
    [InlineData(5, 3, OperatorType.Subtract, 2)]
    [InlineData(5, 3, OperatorType.Multiply, 15)]
    [InlineData(6, 3, OperatorType.Divide, 2)]
    public void ProcessNode_Should_Perform_Arithmetic_Correctly(double left, double right, OperatorType op, double expected)
    {
        // Arrange
        var adapter = new ConsoleAdapter<double>();
        var request = new Request<double>
        {
            Operands =
            {
                ["left"] = new Data<double>(left),
                ["right"] = new Data<double>(right)
            }
        };

        var endNode = new EndNode<double>("End");
        var processNode = new ProcessNode<double>("Result", "left", "right", op, endNode);

        var processor = new Processor<double>(adapter);

        // Act
        processor.Process(processNode, request);

        // Assert
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
    public void DecisionNode_Should_Evaluate_Condition_Correctly(double left, double right, RelationType relation, bool expectYesBranch)
    {
        // Arrange
        var adapter = new ConsoleAdapter<double>();
        var request = new Request<double>
        {
            Operands =
            {
                ["operand"] = new Data<double>(left)
            }
        };

        var yesEndNode = new EndNode<double>("YesEnd");
        var noEndNode = new EndNode<double>("NoEnd");

        var decisionNode = new DecisionNode<double>("Decision", "operand", right, relation, yesEndNode, noEndNode);

        var processor = new Processor<double>(adapter);

        // Act
        processor.Process(decisionNode, request);

        // Assert
        bool yesExecuted = ((Dictionary<Guid, IResponse<double>>)yesEndNode.TypedResultMaps.GetValueOrDefault(typeof(double), new Dictionary<Guid, IResponse<double>>())).Count > 0;
        bool noExecuted = ((Dictionary<Guid, IResponse<double>>)noEndNode.TypedResultMaps.GetValueOrDefault(typeof(double), new Dictionary<Guid, IResponse<double>>())).Count > 0;

        Assert.Equal(expectYesBranch, yesExecuted);
        Assert.Equal(!expectYesBranch, noExecuted);
    }
}
