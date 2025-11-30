using DecisionTreeLib.Data;
using DecisionTreeLib.Node;

namespace DecisionTreeLibTests;

public class ComparatorTests
{
    [Fact]
    public void Compare_WithEqualIntegers_ReturnsZero()
    {
        var left = new Data<int>(5);
        var right = new Data<int>(5);
        
        var result = Comparator.Compare(left, right);
        
        Assert.Equal(0, result);
    }

    [Fact]
    public void Compare_WithLeftLessThanRight_ReturnsNegative()
    {
        var left = new Data<int>(3);
        var right = new Data<int>(5);
        
        var result = Comparator.Compare(left, right);
        
        Assert.True(result < 0);
    }

    [Fact]
    public void Compare_WithLeftGreaterThanRight_ReturnsPositive()
    {
        var left = new Data<int>(7);
        var right = new Data<int>(5);
        
        var result = Comparator.Compare(left, right);
        
        Assert.True(result > 0);
    }

    [Fact]
    public void Compare_WithBothNull_ReturnsZero()
    {
        var left = new Data<string?>(null);
        var right = new Data<string?>(null);
        
        var result = Comparator.Compare(left, right);
        
        Assert.Equal(0, result);
    }

    [Fact]
    public void Compare_WithLeftNull_ReturnsNegative()
    {
        var left = new Data<string?>(null);
        var right = new Data<string>("value");
        
        var result = Comparator.Compare(left, right);
        
        Assert.True(result < 0);
    }

    [Fact]
    public void Compare_WithRightNull_ReturnsPositive()
    {
        var left = new Data<string>("value");
        var right = new Data<string?>(null);
        
        var result = Comparator.Compare(left, right);
        
        Assert.True(result > 0);
    }

    [Fact]
    public void Compare_WithDifferentNumericTypes_ComparesCorrectly()
    {
        var left = new Data<int>(5);
        var right = new Data<double>(5.0);
        
        var result = Comparator.Compare(left, right);
        
        Assert.Equal(0, result);
    }

    [Fact]
    public void Compare_WithFloatAndDouble_ComparesCorrectly()
    {
        var left = new Data<float>(5.0f);
        var right = new Data<double>(5.0);
        
        var result = Comparator.Compare(left, right);
        
        Assert.Equal(0, result);
    }

    [Fact]
    public void Compare_WithStrings_ComparesLexicographically()
    {
        var left = new Data<string>("apple");
        var right = new Data<string>("banana");
        
        var result = Comparator.Compare(left, right);
        
        Assert.True(result < 0);
    }

    [Fact]
    public void Compare_WithEqualStrings_ReturnsZero()
    {
        var left = new Data<string>("test");
        var right = new Data<string>("test");
        
        var result = Comparator.Compare(left, right);
        
        Assert.Equal(0, result);
    }

    [Fact]
    public void Compare_WithUnsupportedTypes_ThrowsNotSupportedException()
    {
        var left = new Data<object>(new object());
        var right = new Data<object>(new object());
        
        Assert.Throws<NotSupportedException>(() => Comparator.Compare(left, right));
    }

    [Theory]
    [InlineData(1, 2, -1)]
    [InlineData(2, 1, 1)]
    [InlineData(10, 10, 0)]
    public void Compare_WithIntegers_ReturnsExpectedResult(int leftVal, int rightVal, int expectedSign)
    {
        var left = new Data<int>(leftVal);
        var right = new Data<int>(rightVal);
        
        var result = Comparator.Compare(left, right);
        
        if (expectedSign < 0)
            Assert.True(result < 0);
        else if (expectedSign > 0)
            Assert.True(result > 0);
        else
            Assert.Equal(0, result);
    }
}

