using DecisionTreeLib.Node;
using DecisionTreeLib.Response;
using DecisionTreeLib.Result;
using Xunit;

namespace Tests;

public class EndNodeTests
{
    [Fact]
    public void EndNode_StoresTitleAndResult()
    {
        var response = new Response<int> { Title = "End", Result = new Result<int> { Value = 42 } };
        var node = new EndNode<int, int, int>("EndNode", response);

        Assert.Equal("EndNode", node.Title);
        Assert.Equal(response, node.Result);
    }

    [Fact]
    public void EndNode_ResultMap_CanAddAndRetrieve()
    {
        var response = new Response<int> { Title = "End", Result = new Result<int> { Value = 42 } };
        var node = new EndNode<int, int, int>("EndNode", response);

        node.ResultMap[node.NodeId] = response;
        Assert.True(node.ResultMap.ContainsKey(node.NodeId));
        Assert.Equal(response, node.ResultMap[node.NodeId]);
    }
}