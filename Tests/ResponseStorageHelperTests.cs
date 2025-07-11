using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Helper;
using DecisionTreeLib.Node;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;
using DecisionTreeLib.Result;

namespace Tests;

public class ResponseStorageHelperTests
{
    private const string ExpectedAddResultTitle = "Add Result";
    private const string ExpectedMultiplyResultTitle = "Multiply Result";
    private const string ExpectedEndResultTitle = "End Result";
    private const int ExpectedAddResultValue = 5;
    private const int ExpectedMultiplyResultValue = 20;
    private const int ExpectedEndNodeResponseValue = 999;
    private const string ExpectedEndResultValue = "Done";
    private const string ExpectedDecisionResultTitle = "Decision Result";
    private const bool ExpectedDecisionResultValue = true;
    private const string ExpectedIntResultTitle = "IntResult";
    private const int ExpectedIntResultValue = 100;
    private const string ExpectedStringResultTitle = "StringResult";
    private const string ExpectedStringResultValue = "Hello";
    private const string ExpectedFirstResultTitle = "First";
    private const int ExpectedFirstResultValue = 1;
    private const string ExpectedSecondResultTitle = "Second";
    private const int ExpectedSecondResultValue = 2;
    private const string ExpectedUpdatedResultTitle = "Updated";
    private const int ExpectedUpdatedResultValue = 99;


    public ResponseStorageHelperTests()
    {
        ResponseStorageHelper.ClearAll();
    }

    [Fact]
    public void MultipleNodeResults_WithCalculationNode_CanBeTracked_Test()
    {
        var endNodeResponse = new Response<int> { Title = ExpectedEndResultTitle, Result = new Result<int> { Value = ExpectedEndNodeResponseValue } };
        var endNode = new EndNode<int, int, int>("End", endNodeResponse);
        
        var request1 = new BinaryOperationRequest<int, int>
        {
            LeftOperand = new Data<int>(2),
            RightOperand = new Data<int>(3),
            Operator = OperatorType.Add
        };
        var request2 = new BinaryOperationRequest<int, int>
        {
            LeftOperand = new Data<int>(4),
            RightOperand = new Data<int>(5),
            Operator = OperatorType.Multiply
        };
        
        var calculationNode2 = new CalculationNode<int, int, int>("Multiply", request2, endNode);
        var calculationNode1 = new CalculationNode<int, int, int>("Add", request1, calculationNode2);
        
        var nodeId1 = calculationNode1.NodeId;
        var nodeId2 = calculationNode2.NodeId;
        var nodeId3 = endNode.NodeId;
        
        var response1 = new Response<int> { Title = ExpectedAddResultTitle, Result = new Result<int> { Value = ExpectedAddResultValue } };
        var response2 = new Response<int> { Title = ExpectedMultiplyResultTitle, Result = new Result<int> { Value = ExpectedMultiplyResultValue } };
        var response3 = new Response<string> { Title = ExpectedEndResultTitle, Result = new Result<string> { Value = ExpectedEndResultValue } };
        
        ResponseStorageHelper.AddResult(nodeId1, response1);
        ResponseStorageHelper.AddResult(nodeId2, response2);
        ResponseStorageHelper.AddResult(nodeId3, response3);
        
        Assert.True(ResponseStorageHelper.TryGetResult(nodeId1, out IResponse<int>? retrieved1));
        Assert.NotNull(retrieved1);
        Assert.Equal(ExpectedAddResultTitle, retrieved1!.Title);
        Assert.Equal(ExpectedAddResultValue, retrieved1.Result!.Value);

        Assert.True(ResponseStorageHelper.TryGetResult(nodeId2, out IResponse<int>? retrieved2));
        Assert.NotNull(retrieved2);
        Assert.Equal(ExpectedMultiplyResultTitle, retrieved2!.Title);
        Assert.Equal(ExpectedMultiplyResultValue, retrieved2.Result!.Value);

        Assert.True(ResponseStorageHelper.TryGetResult(nodeId3, out IResponse<string>? retrieved3));
        Assert.NotNull(retrieved3);
        Assert.Equal(ExpectedEndResultTitle, retrieved3!.Title);
        Assert.Equal(ExpectedEndResultValue, retrieved3.Result!.Value);
    }
    
    [Fact]
    public void MultipleNodeResults_WithDecisionNode_CanBeTracked_Test()
    {
        var endNodeYes = new EndNode<int, int, int>("EndYes", new Response<int> { Title = ExpectedEndResultTitle, Result = new Result<int> { Value = ExpectedEndNodeResponseValue } });
        var endNodeNo = new EndNode<int, int, int>("EndNo", new Response<int> { Title = ExpectedEndResultTitle, Result = new Result<int> { Value = ExpectedEndNodeResponseValue } });

        var decisionRequest = new DecisionRequest<int, int>(new Data<int>(10), new Data<int>(5), RelationType.GreaterThan);
        var decisionNode = new DecisionNode<int, int, int>("Decision", decisionRequest, endNodeYes, endNodeNo);

        var nodeIdDecision = decisionNode.NodeId;
        var nodeIdYes = endNodeYes.NodeId;
        var nodeIdNo = endNodeNo.NodeId;

        var responseDecision = new Response<bool> { Title = ExpectedDecisionResultTitle, Result = new Result<bool> { Value = ExpectedDecisionResultValue } };
        var responseYes = new Response<int> { Title = ExpectedEndResultTitle, Result = new Result<int> { Value = ExpectedEndNodeResponseValue } };
        var responseNo = new Response<int> { Title = ExpectedEndResultTitle, Result = new Result<int> { Value = ExpectedEndNodeResponseValue } };

        ResponseStorageHelper.AddResult(nodeIdDecision, responseDecision);
        ResponseStorageHelper.AddResult(nodeIdYes, responseYes);
        ResponseStorageHelper.AddResult(nodeIdNo, responseNo);

        Assert.True(ResponseStorageHelper.TryGetResult(nodeIdDecision, out IResponse<bool>? retrievedDecision));
        Assert.NotNull(retrievedDecision);
        Assert.Equal(ExpectedDecisionResultTitle, retrievedDecision!.Title);
        Assert.Equal(ExpectedDecisionResultValue, retrievedDecision.Result!.Value);

        Assert.True(ResponseStorageHelper.TryGetResult(nodeIdYes, out IResponse<int>? retrievedYes));
        Assert.NotNull(retrievedYes);
        Assert.Equal(ExpectedEndResultTitle, retrievedYes!.Title);
        Assert.Equal(ExpectedEndNodeResponseValue, retrievedYes.Result!.Value);

        Assert.True(ResponseStorageHelper.TryGetResult(nodeIdNo, out IResponse<int>? retrievedNo));
        Assert.NotNull(retrievedNo);
        Assert.Equal(ExpectedEndResultTitle, retrievedNo!.Title);
        Assert.Equal(ExpectedEndNodeResponseValue, retrievedNo.Result!.Value);
    }

    [Fact]
    public void MultipleNodeResults_WithMixedProcessAndDecisionNodes_CanBeTracked_Test()
    {
        var endNodeYes = new EndNode<int, int, int>("EndYes", new Response<int> { Title = ExpectedEndResultTitle, Result = new Result<int> { Value = ExpectedEndNodeResponseValue } });
        var endNodeNo = new EndNode<int, int, int>("EndNo", new Response<int> { Title = ExpectedEndResultTitle, Result = new Result<int> { Value = ExpectedEndNodeResponseValue } });

        var decisionRequest = new DecisionRequest<int, int>(new Data<int>(10), new Data<int>(5), RelationType.GreaterThan);
        var decisionNode = new DecisionNode<int, int, int>("Decision", decisionRequest, endNodeYes, endNodeNo);

        var processRequest = new BinaryOperationRequest<int, int>
        {
            LeftOperand = new Data<int>(2),
            RightOperand = new Data<int>(3),
            Operator = OperatorType.Add
        };
        var calculationNode = new CalculationNode<int, int, int>("Add", processRequest, decisionNode);

        var nodeIdProcess = calculationNode.NodeId;
        var nodeIdDecision = decisionNode.NodeId;
        var nodeIdYes = endNodeYes.NodeId;
        var nodeIdNo = endNodeNo.NodeId;

        var responseProcess = new Response<int> { Title = ExpectedAddResultTitle, Result = new Result<int> { Value = ExpectedAddResultValue } };
        var responseDecision = new Response<bool> { Title = ExpectedDecisionResultTitle, Result = new Result<bool> { Value = ExpectedDecisionResultValue } };
        var responseYes = new Response<int> { Title = ExpectedEndResultTitle, Result = new Result<int> { Value = ExpectedEndNodeResponseValue } };
        var responseNo = new Response<int> { Title = ExpectedEndResultTitle, Result = new Result<int> { Value = ExpectedEndNodeResponseValue } };

        ResponseStorageHelper.AddResult(nodeIdProcess, responseProcess);
        ResponseStorageHelper.AddResult(nodeIdDecision, responseDecision);
        ResponseStorageHelper.AddResult(nodeIdYes, responseYes);
        ResponseStorageHelper.AddResult(nodeIdNo, responseNo);

        Assert.True(ResponseStorageHelper.TryGetResult(nodeIdProcess, out IResponse<int>? retrievedProcess));
        Assert.NotNull(retrievedProcess);
        Assert.Equal(ExpectedAddResultTitle, retrievedProcess!.Title);
        Assert.Equal(ExpectedAddResultValue, retrievedProcess.Result!.Value);

        Assert.True(ResponseStorageHelper.TryGetResult(nodeIdDecision, out IResponse<bool>? retrievedDecision));
        Assert.NotNull(retrievedDecision);
        Assert.Equal(ExpectedDecisionResultTitle, retrievedDecision!.Title);
        Assert.Equal(ExpectedDecisionResultValue, retrievedDecision.Result!.Value);

        Assert.True(ResponseStorageHelper.TryGetResult(nodeIdYes, out IResponse<int>? retrievedYes));
        Assert.NotNull(retrievedYes);
        Assert.Equal(ExpectedEndResultTitle, retrievedYes!.Title);
        Assert.Equal(ExpectedEndNodeResponseValue, retrievedYes.Result!.Value);

        Assert.True(ResponseStorageHelper.TryGetResult(nodeIdNo, out IResponse<int>? retrievedNo));
        Assert.NotNull(retrievedNo);
        Assert.Equal(ExpectedEndResultTitle, retrievedNo!.Title);
        Assert.Equal(ExpectedEndNodeResponseValue, retrievedNo.Result!.Value);
    }    

    [Fact]
    public void ClearAll_RemovesAllStoredResults_Test()
    {
        var nodeId1 = Guid.NewGuid();
        var nodeId2 = Guid.NewGuid();

        var response1 = new Response<int> { Title = ExpectedAddResultTitle, Result = new Result<int> { Value = ExpectedAddResultValue } };
        var response2 = new Response<int> { Title = ExpectedMultiplyResultTitle, Result = new Result<int> { Value = ExpectedMultiplyResultValue } };

        ResponseStorageHelper.AddResult(nodeId1, response1);
        ResponseStorageHelper.AddResult(nodeId2, response2);

        Assert.True(ResponseStorageHelper.TryGetResult<int>(nodeId1, out _));
        Assert.True(ResponseStorageHelper.TryGetResult<int>(nodeId2, out _));

        ResponseStorageHelper.ClearAll();

        Assert.False(ResponseStorageHelper.TryGetResult<int>(nodeId1, out _));
        Assert.False(ResponseStorageHelper.TryGetResult<int>(nodeId2, out _));
    }
    
    [Fact]
    public void ClearResultsForType_RemovesOnlySpecifiedType_Test()
    {
        var nodeIdInt = Guid.NewGuid();
        var nodeIdString = Guid.NewGuid();

        ResponseStorageHelper.AddResult(nodeIdInt, new Response<int> { Title = ExpectedIntResultTitle, Result = new Result<int> { Value = ExpectedIntResultValue } });
        ResponseStorageHelper.AddResult(nodeIdString, new Response<string> { Title = ExpectedStringResultTitle, Result = new Result<string> { Value = ExpectedStringResultValue } });

        ResponseStorageHelper.ClearResultsForType<int>();

        Assert.False(ResponseStorageHelper.TryGetResult<int>(nodeIdInt, out _));
        Assert.True(ResponseStorageHelper.TryGetResult<string>(nodeIdString, out var stringResp));
        Assert.NotNull(stringResp);
        Assert.Equal(ExpectedStringResultTitle, stringResp!.Title);
        Assert.Equal(ExpectedStringResultValue, stringResp.Result!.Value);
    }

    [Fact]
    public void GetResultMap_ReturnsAllStoredResultsOfType_Test()
    {
        var nodeId1 = Guid.NewGuid();
        var nodeId2 = Guid.NewGuid();

        ResponseStorageHelper.AddResult(nodeId1, new Response<int> { Title = ExpectedFirstResultTitle, Result = new Result<int> { Value = ExpectedFirstResultValue } });
        ResponseStorageHelper.AddResult(nodeId2, new Response<int> { Title = ExpectedSecondResultTitle, Result = new Result<int> { Value = ExpectedSecondResultValue } });

        var map = ResponseStorageHelper.GetResultMap<int>();

        Assert.NotNull(map);
        Assert.Equal(2, map!.Count);
        Assert.True(map.ContainsKey(nodeId1));
        Assert.True(map.ContainsKey(nodeId2));
    }

    [Fact]
    public void AddResult_OverwritesExistingResult_Test()
    {
        var nodeId = Guid.NewGuid();

        ResponseStorageHelper.AddResult(nodeId, new Response<int> { Title = ExpectedFirstResultTitle, Result = new Result<int> { Value = ExpectedFirstResultValue } });
        ResponseStorageHelper.AddResult(nodeId, new Response<int> { Title = ExpectedUpdatedResultTitle, Result = new Result<int> { Value = ExpectedUpdatedResultValue } });

        Assert.True(ResponseStorageHelper.TryGetResult<int>(nodeId, out var result));
        Assert.NotNull(result);
        Assert.Equal(ExpectedUpdatedResultTitle, result!.Title);
        Assert.Equal(ExpectedUpdatedResultValue, result.Result!.Value);
    }

    [Fact]
    public void TryGetResult_ReturnsFalse_WhenKeyDoesNotExist_Test()
    {
        var nodeId = Guid.NewGuid();
        var found = ResponseStorageHelper.TryGetResult<int>(nodeId, out _);

        Assert.False(found);
    }
}