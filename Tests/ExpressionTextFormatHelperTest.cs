using DecisionTreeLib.Helper;
using Xunit;

namespace Tests;

public class ExpressionTextFormatHelperTest
{
    [Fact]
    public void FormatOperation_ReturnsCorrectString_Test()
    {
        var left = 5;
        var operation = "+";
        var right = 3;
        var result = 8;

        var formatted = ExpressionTextFormatHelper.FormatOperation(left, operation, right, result);

        Assert.Equal("5 + 3 = 8", formatted);
    }

    [Fact]
    public void FormatOperation_WorksWithStrings_Test()
    {
        var left = "A";
        var operation = "concat";
        var right = "B";
        var result = "AB";

        var formatted = ExpressionTextFormatHelper.FormatOperation(left, operation, right, result);

        Assert.Equal("A concat B = AB", formatted);
    }

    [Fact]
    public void FormatRelation_ReturnsCorrectString_Test()
    {
        var left = 10;
        var relation = ">";
        var right = 5;
        var condition = true;

        var formatted = ExpressionTextFormatHelper.FormatRelation(left, relation, right, condition);

        Assert.Equal("10 > 5 = True", formatted);
    }

    [Fact]
    public void FormatRelation_ReturnsCorrectStringWithFalse_Test()
    {
        var left = 2;
        var relation = "<";
        var right = 1;
        var condition = false;

        var formatted = ExpressionTextFormatHelper.FormatRelation(left, relation, right, condition);

        Assert.Equal("2 < 1 = False", formatted);
    }
}