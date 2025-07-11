using DecisionTreeLib.Node;
using DecisionTreeLib.Request;
using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using Xunit;

namespace Tests;

public class DecisionNodeTests
{
    [Fact]
    public void DecisionNode_StoresProperties()
    {
        var request = new DecisionRequest<int, int>(new Data<int>(1), new Data<int>(2), RelationType.Equal);
        var yesNode = new EndNode<int, int, int>("Yes", result: new DecisionTreeLib.Response.Response<int>
        {
            Title = ""
        });
        var noNode = new EndNode<int, int, int>("No", result: new DecisionTreeLib.Response.Response<int>
        {
            Title = ""
        });
        var node = new DecisionNode<int, int, int>("Decision", request, yesNode, noNode);

        Assert.Equal("Decision", node.Title);
        Assert.Equal(request, node.Request);
        Assert.Equal(yesNode, node.YesNextNode);
        Assert.Equal(noNode, node.NoNextNode);
    }

    [Fact]
    public void DecisionNode_ResultMap_CanAddAndRetrieve()
    {
        var request = new DecisionRequest<int, int>(new Data<int>(1), new Data<int>(2), RelationType.Equal);
        var yesNode = new EndNode<int, int, int>("Yes", result: new DecisionTreeLib.Response.Response<int>
        {
            Title = ""
        });
        var noNode = new EndNode<int, int, int>("No", result: new DecisionTreeLib.Response.Response<int>
        {
            Title = ""
        });
        var node = new DecisionNode<int, int, int>("Decision", request, yesNode, noNode);

        var response = new DecisionTreeLib.Response.Response<int>
        {
            Title = ""
        };
        node.ResultMap[node.NodeId] = response;
        Assert.Equal(response, node.ResultMap[node.NodeId]);
    }
}