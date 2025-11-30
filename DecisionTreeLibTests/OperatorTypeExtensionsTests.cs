using DecisionTreeLib.Enums;
using DecisionTreeLib.Extensions;

namespace DecisionTreeLibTests;

public class OperatorTypeExtensionsTests
{
    [Theory]
    [InlineData(OperatorType.Add, "+")]
    [InlineData(OperatorType.Subtract, "-")]
    [InlineData(OperatorType.Multiply, "*")]
    [InlineData(OperatorType.Divide, "/")]
    [InlineData(OperatorType.And, "&&")]
    [InlineData(OperatorType.Or, "||")]
    [InlineData(OperatorType.Not, "!")]
    [InlineData(OperatorType.Xor, "^")]
    [InlineData(OperatorType.Nand, "!&&")]
    [InlineData(OperatorType.Nor, "!||")]
    public void ToSymbol_WithValidOperator_ReturnsCorrectSymbol(OperatorType op, string expected)
    {
        var result = op.ToSymbol();

        Assert.Equal(expected, result);
    }
}

