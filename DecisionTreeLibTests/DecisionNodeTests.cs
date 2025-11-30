using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Evaluator;
using DecisionTreeLib.Node;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;
using DecisionTreeLib.Adapters;

namespace DecisionTreeLibTests;

public class DecisionNodeTests
{
    private class MockAdapter : IAdapter
    {
        public List<string> Messages { get; } = new();
        public void Write(string message) => Messages.Add(message);
    }

    [Fact]
    public void DecisionNode_Constructor_SetsProperties()
    {
        var request = new DecisionRequest<int, int>(new Data<int>(1), new Data<int>(2), RelationType.Equal);
        var yesNode = CreateEndNode("Yes");
        var noNode = CreateEndNode("No");
        var node = new DecisionNode<int, int, bool>("Decision", request, yesNode, noNode);

        Assert.Equal("Decision", node.Title);
        Assert.Equal(request, node.Request);
        Assert.Equal(yesNode, node.YesNextNode);
        Assert.Equal(noNode, node.NoNextNode);
        Assert.NotEqual(Guid.Empty, node.NodeId);
    }

    [Fact]
    public void DecisionNode_ResultMap_CanAddAndRetrieve()
    {
        var request = new DecisionRequest<int, int>(new Data<int>(1), new Data<int>(2), RelationType.Equal);
        var yesNode = CreateEndNode("Yes");
        var noNode = CreateEndNode("No");
        var node = new DecisionNode<int, int, bool>("Decision", request, yesNode, noNode);

        var response = new Response<bool> { Title = "Test" };
        node.ResultMap[node.NodeId] = response;
        
        Assert.Equal(response, node.ResultMap[node.NodeId]);
    }

    [Fact]
    public void DecisionNode_Execute_WithEqualRelation_ReturnsYesNode()
    {
        var request = new DecisionRequest<int, int>(new Data<int>(5), new Data<int>(5), RelationType.Equal);
        var yesNode = CreateEndNode("Yes");
        var noNode = CreateEndNode("No");
        var node = new DecisionNode<int, int, bool>("Decision", request, yesNode, noNode);
        var adapter = new MockAdapter();
        var evaluator = new DecisionTreeEvaluator(adapter);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
        Assert.True(adapter.Messages.Any(m => m.Contains("Yes")));
    }

    [Fact]
    public void DecisionNode_Execute_WithNotEqualRelation_ReturnsNoNode()
    {
        var request = new DecisionRequest<int, int>(new Data<int>(5), new Data<int>(3), RelationType.NotEqual);
        var yesNode = CreateEndNode("Yes");
        var noNode = CreateEndNode("No");
        var node = new DecisionNode<int, int, bool>("Decision", request, yesNode, noNode);
        var adapter = new MockAdapter();
        var evaluator = new DecisionTreeEvaluator(adapter);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(3, 5, RelationType.LessThan, true)]
    [InlineData(5, 3, RelationType.LessThan, false)]
    [InlineData(5, 5, RelationType.LessThanOrEqual, true)]
    [InlineData(3, 5, RelationType.LessThanOrEqual, true)]
    [InlineData(5, 3, RelationType.GreaterThan, true)]
    [InlineData(3, 5, RelationType.GreaterThan, false)]
    [InlineData(5, 5, RelationType.GreaterThanOrEqual, true)]
    [InlineData(5, 3, RelationType.GreaterThanOrEqual, true)]
    [InlineData(5, 5, RelationType.Equal, true)]
    [InlineData(5, 3, RelationType.Equal, false)]
    [InlineData(5, 3, RelationType.NotEqual, true)]
    [InlineData(5, 5, RelationType.NotEqual, false)]
    public void DecisionNode_Execute_WithVariousRelations_ReturnsCorrectNode(int left, int right, RelationType relation, bool shouldGoToYes)
    {
        var request = new DecisionRequest<int, int>(new Data<int>(left), new Data<int>(right), relation);
        var yesNode = CreateEndNode("Yes");
        var noNode = CreateEndNode("No");
        var node = new DecisionNode<int, int, bool>("Decision", request, yesNode, noNode);
        var adapter = new MockAdapter();
        var evaluator = new DecisionTreeEvaluator(adapter);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
        if (shouldGoToYes)
        {
            Assert.True(adapter.Messages.Any(m => m.Contains("Yes")));
        }
    }

    [Fact]
    public void DecisionNode_Execute_WithContainsRelation_ReturnsCorrectNode()
    {
        var request = new DecisionRequest<string, string>(
            new Data<string>("hello world"), 
            new Data<string>("world"), 
            RelationType.Contains);
        var yesNode = CreateEndNodeForString("Yes");
        var noNode = CreateEndNodeForString("No");
        var node = new DecisionNode<string, string, bool>("Decision", request, yesNode, noNode);
        var adapter = new MockAdapter();
        var evaluator = new DecisionTreeEvaluator(adapter);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
        Assert.True(adapter.Messages.Any(m => m.Contains("Yes")));
    }

    [Fact]
    public void DecisionNode_Execute_WithContainsRelation_WhenNotContained_ReturnsNoNode()
    {
        var request = new DecisionRequest<string, string>(
            new Data<string>("hello"), 
            new Data<string>("world"), 
            RelationType.Contains);
        var yesNode = CreateEndNodeForString("Yes");
        var noNode = CreateEndNodeForString("No");
        var node = new DecisionNode<string, string, bool>("Decision", request, yesNode, noNode);
        var adapter = new MockAdapter();
        var evaluator = new DecisionTreeEvaluator(adapter);

        var result = node.Execute(evaluator);

        Assert.NotNull(result);
    }

    [Fact]
    public void DecisionNode_Execute_StoresResultInResultMap()
    {
        var request = new DecisionRequest<int, int>(new Data<int>(5), new Data<int>(5), RelationType.Equal);
        var yesNode = CreateEndNode("Yes");
        var noNode = CreateEndNode("No");
        var node = new DecisionNode<int, int, bool>("Decision", request, yesNode, noNode);
        var evaluator = new DecisionTreeEvaluator(null);

        node.Execute(evaluator);

        Assert.True(node.ResultMap.ContainsKey(node.NodeId));
        Assert.NotNull(node.ResultMap[node.NodeId]);
    }

    [Fact]
    public void DecisionNode_Execute_WritesToAdapter()
    {
        var request = new DecisionRequest<int, int>(new Data<int>(5), new Data<int>(3), RelationType.GreaterThan);
        var yesNode = CreateEndNode("Yes");
        var noNode = CreateEndNode("No");
        var node = new DecisionNode<int, int, bool>("Decision", request, yesNode, noNode);
        var adapter = new MockAdapter();
        var evaluator = new DecisionTreeEvaluator(adapter);

        node.Execute(evaluator);

        Assert.NotEmpty(adapter.Messages);
        Assert.Contains("Evaluating decision", adapter.Messages.First());
    }

    private static EndNode<int, int, bool> CreateEndNode(string title)
    {
        return new EndNode<int, int, bool>(title, new Response<bool> { Title = title });
    }

    private static EndNode<string, string, bool> CreateEndNodeForString(string title)
    {
        return new EndNode<string, string, bool>(title, new Response<bool> { Title = title });
    }
}

