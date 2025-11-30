using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Request;

namespace DecisionTreeLibTests;

public class DecisionRequestTests
{
    [Fact]
    public void DecisionRequest_Constructor_SetsProperties()
    {
        var left = new Data<int>(5);
        var right = new Data<int>(3);
        var request = new DecisionRequest<int, int>(left, right, RelationType.Equal);

        Assert.Equal(left, request.LeftOperand);
        Assert.Equal(right, request.RightOperand);
        Assert.Equal(RelationType.Equal, request.Relation);
    }

    [Fact]
    public void DecisionRequest_WithDifferentTypes_StoresCorrectly()
    {
        var left = new Data<int>(5);
        var right = new Data<double>(3.5);
        var request = new DecisionRequest<int, double>(left, right, RelationType.LessThan);

        Assert.Equal(left, request.LeftOperand);
        Assert.Equal(right, request.RightOperand);
        Assert.Equal(RelationType.LessThan, request.Relation);
    }
}

public class BinaryOperationRequestTests
{
    [Fact]
    public void BinaryOperationRequest_Properties_CanBeSet()
    {
        var request = new BinaryOperationRequest<int, int>
        {
            LeftOperand = new Data<int>(5),
            RightOperand = new Data<int>(3),
            Operator = OperatorType.Add
        };

        Assert.Equal(5, request.LeftOperand.Value);
        Assert.Equal(3, request.RightOperand.Value);
        Assert.Equal(OperatorType.Add, request.Operator);
    }

    [Fact]
    public void BinaryOperationRequest_WithDifferentTypes_StoresCorrectly()
    {
        var request = new BinaryOperationRequest<int, double>
        {
            LeftOperand = new Data<int>(5),
            RightOperand = new Data<double>(3.5),
            Operator = OperatorType.Multiply
        };

        Assert.Equal(5, request.LeftOperand.Value);
        Assert.Equal(3.5, request.RightOperand.Value);
        Assert.Equal(OperatorType.Multiply, request.Operator);
    }

    [Fact]
    public void BinaryOperationRequest_Properties_CanBeModified()
    {
        var request = new BinaryOperationRequest<int, int>
        {
            LeftOperand = new Data<int>(5),
            RightOperand = new Data<int>(3),
            Operator = OperatorType.Add
        };

        request.LeftOperand = new Data<int>(10);
        request.RightOperand = new Data<int>(20);
        request.Operator = OperatorType.Subtract;

        Assert.Equal(10, request.LeftOperand.Value);
        Assert.Equal(20, request.RightOperand.Value);
        Assert.Equal(OperatorType.Subtract, request.Operator);
    }
}

public class UnaryOperationRequestTests
{
    [Fact]
    public void UnaryOperationRequest_Properties_CanBeSet()
    {
        var request = new UnaryOperationRequest<int>
        {
            Operand = new Data<int>(5),
            Operator = OperatorType.Not
        };

        Assert.Equal(5, request.Operand.Value);
        Assert.Equal(OperatorType.Not, request.Operator);
    }

    [Fact]
    public void UnaryOperationRequest_WithDifferentTypes_StoresCorrectly()
    {
        var request = new UnaryOperationRequest<double>
        {
            Operand = new Data<double>(3.14),
            Operator = OperatorType.Not
        };

        Assert.Equal(3.14, request.Operand.Value);
        Assert.Equal(OperatorType.Not, request.Operator);
    }

    [Fact]
    public void UnaryOperationRequest_Properties_CanBeModified()
    {
        var request = new UnaryOperationRequest<int>
        {
            Operand = new Data<int>(5),
            Operator = OperatorType.Not
        };

        request.Operand = new Data<int>(10);
        request.Operator = OperatorType.Not;

        Assert.Equal(10, request.Operand.Value);
        Assert.Equal(OperatorType.Not, request.Operator);
    }
}

