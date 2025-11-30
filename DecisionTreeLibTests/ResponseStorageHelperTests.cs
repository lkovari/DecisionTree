using DecisionTreeLib.Helper;
using DecisionTreeLib.Response;

namespace DecisionTreeLibTests;

public class ResponseStorageHelperTests
{
    [Fact]
    public void AddResult_WithNewType_CreatesNewMap()
    {
        ResponseStorageHelper.ClearAll();
        var nodeId = Guid.NewGuid();
        var response = new Response<int> { Title = "Test", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };

        ResponseStorageHelper.AddResult(nodeId, response);

        Assert.True(ResponseStorageHelper.TryGetResult<int>(nodeId, out var retrieved));
        Assert.NotNull(retrieved);
        Assert.Equal(42, retrieved!.Result!.Value);
    }

    [Fact]
    public void AddResult_WithExistingType_AddsToExistingMap()
    {
        ResponseStorageHelper.ClearAll();
        var nodeId1 = Guid.NewGuid();
        var nodeId2 = Guid.NewGuid();
        var response1 = new Response<int> { Title = "Test1", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };
        var response2 = new Response<int> { Title = "Test2", Result = new DecisionTreeLib.Result.Result<int> { Value = 100 } };

        ResponseStorageHelper.AddResult(nodeId1, response1);
        ResponseStorageHelper.AddResult(nodeId2, response2);

        Assert.True(ResponseStorageHelper.TryGetResult<int>(nodeId1, out var retrieved1));
        Assert.True(ResponseStorageHelper.TryGetResult<int>(nodeId2, out var retrieved2));
        Assert.Equal(42, retrieved1!.Result!.Value);
        Assert.Equal(100, retrieved2!.Result!.Value);
    }

    [Fact]
    public void TryGetResult_WithNonExistentNodeId_ReturnsFalse()
    {
        ResponseStorageHelper.ClearAll();
        var nodeId = Guid.NewGuid();

        var result = ResponseStorageHelper.TryGetResult<int>(nodeId, out var response);

        Assert.False(result);
        Assert.Null(response);
    }

    [Fact]
    public void TryGetResult_WithDifferentType_ReturnsFalse()
    {
        ResponseStorageHelper.ClearAll();
        var nodeId = Guid.NewGuid();
        var response = new Response<int> { Title = "Test", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };

        ResponseStorageHelper.AddResult(nodeId, response);

        var result = ResponseStorageHelper.TryGetResult<string>(nodeId, out var retrieved);

        Assert.False(result);
        Assert.Null(retrieved);
    }

    [Fact]
    public void ClearAll_RemovesAllResults()
    {
        ResponseStorageHelper.ClearAll();
        var nodeId1 = Guid.NewGuid();
        var nodeId2 = Guid.NewGuid();
        var response1 = new Response<int> { Title = "Test1", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };
        var response2 = new Response<string> { Title = "Test2", Result = new DecisionTreeLib.Result.Result<string> { Value = "test" } };

        ResponseStorageHelper.AddResult(nodeId1, response1);
        ResponseStorageHelper.AddResult(nodeId2, response2);
        ResponseStorageHelper.ClearAll();

        Assert.False(ResponseStorageHelper.TryGetResult<int>(nodeId1, out _));
        Assert.False(ResponseStorageHelper.TryGetResult<string>(nodeId2, out _));
    }

    [Fact]
    public void ClearResultsForType_RemovesOnlySpecifiedType()
    {
        ResponseStorageHelper.ClearAll();
        var nodeId1 = Guid.NewGuid();
        var nodeId2 = Guid.NewGuid();
        var response1 = new Response<int> { Title = "Test1", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };
        var response2 = new Response<string> { Title = "Test2", Result = new DecisionTreeLib.Result.Result<string> { Value = "test" } };

        ResponseStorageHelper.AddResult(nodeId1, response1);
        ResponseStorageHelper.AddResult(nodeId2, response2);
        ResponseStorageHelper.ClearResultsForType<int>();

        Assert.False(ResponseStorageHelper.TryGetResult<int>(nodeId1, out _));
        Assert.True(ResponseStorageHelper.TryGetResult<string>(nodeId2, out _));
    }

    [Fact]
    public void GetResultMap_WithExistingType_ReturnsMap()
    {
        ResponseStorageHelper.ClearAll();
        var nodeId = Guid.NewGuid();
        var response = new Response<int> { Title = "Test", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };

        ResponseStorageHelper.AddResult(nodeId, response);
        var map = ResponseStorageHelper.GetResultMap<int>();

        Assert.NotNull(map);
        Assert.True(map!.ContainsKey(nodeId));
        Assert.Equal(42, map[nodeId].Result!.Value);
    }

    [Fact]
    public void GetResultMap_WithNonExistentType_ReturnsNull()
    {
        ResponseStorageHelper.ClearAll();

        var map = ResponseStorageHelper.GetResultMap<int>();

        Assert.Null(map);
    }

    [Fact]
    public void AddResult_OverwritesExistingNodeId()
    {
        ResponseStorageHelper.ClearAll();
        var nodeId = Guid.NewGuid();
        var response1 = new Response<int> { Title = "Test1", Result = new DecisionTreeLib.Result.Result<int> { Value = 42 } };
        var response2 = new Response<int> { Title = "Test2", Result = new DecisionTreeLib.Result.Result<int> { Value = 100 } };

        ResponseStorageHelper.AddResult(nodeId, response1);
        ResponseStorageHelper.AddResult(nodeId, response2);

        Assert.True(ResponseStorageHelper.TryGetResult<int>(nodeId, out var retrieved));
        Assert.Equal(100, retrieved!.Result!.Value);
    }
}

