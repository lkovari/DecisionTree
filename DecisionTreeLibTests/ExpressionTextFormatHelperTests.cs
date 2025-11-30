using DecisionTreeLib.Helper;

namespace DecisionTreeLibTests;

public class ExpressionTextFormatHelperTests
{
    [Fact]
    public void FormatOperation_WithIntegers_ReturnsFormattedString()
    {
        var result = ExpressionTextFormatHelper.FormatOperation(5, "+", 3, 8);

        Assert.Equal("5 + 3 = 8", result);
    }

    [Fact]
    public void FormatOperation_WithFloats_ReturnsFormattedString()
    {
        var result = ExpressionTextFormatHelper.FormatOperation(5.5, "*", 2.0, 11.0);

        Assert.Contains("5", result);
        Assert.Contains("*", result);
        Assert.Contains("2", result);
        Assert.Contains("=", result);
        Assert.Contains("11", result);
    }

    [Fact]
    public void FormatOperation_WithStrings_ReturnsFormattedString()
    {
        var result = ExpressionTextFormatHelper.FormatOperation("hello", "+", "world", "helloworld");

        Assert.Equal("hello + world = helloworld", result);
    }

    [Fact]
    public void FormatOperation_WithNullValues_ReturnsFormattedString()
    {
        object? left = null;
        object? right = null;
        object? res = null;
        var result = ExpressionTextFormatHelper.FormatOperation(left!, "+", right!, res!);

        Assert.Equal(" +  = ", result);
    }

    [Fact]
    public void FormatRelation_WithTrueCondition_ReturnsFormattedString()
    {
        var result = ExpressionTextFormatHelper.FormatRelation(5, "Equal", 5, true);

        Assert.Equal("5 Equal 5 = True", result);
    }

    [Fact]
    public void FormatRelation_WithFalseCondition_ReturnsFormattedString()
    {
        var result = ExpressionTextFormatHelper.FormatRelation(5, "LessThan", 3, false);

        Assert.Equal("5 LessThan 3 = False", result);
    }

    [Fact]
    public void FormatRelation_WithStrings_ReturnsFormattedString()
    {
        var result = ExpressionTextFormatHelper.FormatRelation("apple", "Contains", "app", true);

        Assert.Equal("apple Contains app = True", result);
    }

    [Fact]
    public void FormatRelation_WithNullValues_ReturnsFormattedString()
    {
        object? left = null;
        object? right = null;
        var result = ExpressionTextFormatHelper.FormatRelation(left!, "Equal", right!, false);

        Assert.Equal(" Equal  = False", result);
    }
}

