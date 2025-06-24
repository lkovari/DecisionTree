
using DecisionTreeLib.Adapters;
using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Node;
using DecisionTreeLib.Processing;
using DecisionTreeLib.Request;

namespace Tests;

public class ProcessAndDecisionNodeTests
{
    private readonly IAdapter<double> _adapter = new ConsoleAdapter<double>();

    [Theory]
    [InlineData(5, 3, OperatorType.Add, 8)]
    [InlineData(5, 3, OperatorType.Subtract, 2)]
    [InlineData(5, 3, OperatorType.Multiply, 15)]
    [InlineData(6, 3, OperatorType.Divide, 2)]
    public void ProcessNode_Should_Calculate_Correct_Result_And_Invoke_NextNode(double left, double right, OperatorType op, double expected)
    {
        var request = new Request<double>
        {
            Operands = new Dictionary<string, IData<double>>
            {
                { "Left", new Data<double>(left) },
                { "Right", new Data<double>(right) }
            }
        };

        var nextNodeExecuted = false;

        var nextNode = new DummyNode<double>(() => nextNodeExecuted = true, "DummyProcessNode");

        var processNode = new ProcessNode<double>("TestNode", "Left", "Right", op, nextNode);
        
        var processor = new Processor<double>(_adapter);
        processor.Process(processNode, request);
        
        // Assert
        Assert.Contains(processNode.NodeId, processNode.ResultMap);
        Assert.NotNull(processNode.ResultMap[processNode.NodeId].Result);
        Assert.Equal(expected, processNode.ResultMap[processNode.NodeId].Result!.Value);
        Assert.True(nextNodeExecuted);
    }

    [Theory]
    [InlineData(5, 5, RelationType.Equal, true)]
    [InlineData(5, 3, RelationType.GreaterThan, true)]
    [InlineData(3, 5, RelationType.LessThan, true)]
    [InlineData(5, 5, RelationType.GreaterThanOrEqual, true)]
    [InlineData(3, 5, RelationType.LessThanOrEqual, true)]
    [InlineData(5, 3, RelationType.Equal, false)]
    public void DecisionNode_Should_Evaluate_Correctly_And_Invoke_Proper_NextNode(double operandValue, double compareValue, RelationType relationType, bool expectYesBranch)
    {
        bool yesNodeExecuted = false;
        bool noNodeExecuted = false;

        var yesNode = new DummyNode<double>(() => yesNodeExecuted = true, "DummyYesDecisionNode");
        var noNode = new DummyNode<double>(() => noNodeExecuted = true, "DummyNoDecisionNode");

        var request = new Request<double>
        {
            Operands = new Dictionary<string, IData<double>>
            {
                { "Operand", new Data<double>(operandValue) }
            }
        };

        var decisionNode = new DecisionNode<double>("TestDecision", "Operand", compareValue, relationType, yesNode, noNode);
        
        var processor = new Processor<double>(_adapter);
        
        processor.Process(decisionNode, request);
        
        // Assert
        if (expectYesBranch)
        {
            Assert.True(yesNodeExecuted);
            Assert.False(noNodeExecuted);
        }
        else
        {
            Assert.False(yesNodeExecuted);
            Assert.True(noNodeExecuted);
        }
    }
}