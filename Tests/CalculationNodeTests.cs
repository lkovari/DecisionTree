
using DecisionTreeLib.Node;
using DecisionTreeLib.Request;
using DecisionTreeLib.Data;
using Xunit;

namespace Tests;

public class CalculationNodeTests
{
    [Fact]
    public void CalculationNode_StoresProperties()
    {
        var request = new OperationRequest<int, int>(new Data<int>(1), new Data<int>(2), DecisionTreeLib.Enums.OperatorType.Add);
        var endNode = new EndNode<int, int, int>("End", new DecisionTreeLib.Response.Response<int>
        {
            Title = ""
        });
        var node = new CalculationNode<int, int, int>("Calc", request, endNode);

        Assert.Equal("Calc", node.Title);
        Assert.Equal(request, node.Request);
        Assert.Equal(endNode, node.NextNode);
    }

    [Fact]
    public void CalculationNode_ResultMap_CanAddAndRetrieve()
    {
        var request = new OperationRequest<int, int>(new Data<int>(1), new Data<int>(2), DecisionTreeLib.Enums.OperatorType.Add);
        var endNode = new EndNode<int, int, int>("End", new DecisionTreeLib.Response.Response<int>
        {
            Title = ""
        });
        var node = new CalculationNode<int, int, int>("Calc", request, endNode);

        var response = new DecisionTreeLib.Response.Response<int>
        {
            Title = ""
        };
        node.ResultMap[node.NodeId] = response;
        Assert.Equal(response, node.ResultMap[node.NodeId]);
    }
}