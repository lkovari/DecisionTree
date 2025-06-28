using DecisionTreeLib.Data;
using DecisionTreeLib.Node;

namespace Tests;

public class ComparatorTests
{
    [Fact]
    public void Compare_BothNull_ReturnsZero_Test()
    {
        var left = new Data<object?>(null);
        var right = new Data<object?>(null);

        var result = Comparator.Compare(left, right);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Compare_LeftNull_ReturnsMinusOne_Test()
    {
        var left = new Data<object?>(null);
        var right = new Data<object?>("abc");

        var result = Comparator.Compare(left, right);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void Compare_RightNull_ReturnsOne_Test()
    {
        var left = new Data<object?>("abc");
        var right = new Data<object?>(null);

        var result = Comparator.Compare(left, right);

        Assert.Equal(1, result);
    }

    [Fact]
    public void Compare_BothNumeric_ReturnsZero_Test()
    {
        var left = new Data<int>(5);
        var right = new Data<double>(5.0);

        var result = Comparator.Compare(left, right);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Compare_LeftLessThanRightNumeric_ReturnsMinusOne_Test()
    {
        var left = new Data<int>(3);
        var right = new Data<double>(4.0);

        var result = Comparator.Compare(left, right);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void Compare_LeftGreaterThanRightNumeric_ReturnsOne_Test()
    {
        var left = new Data<double>(7.5);
        var right = new Data<int>(6);

        var result = Comparator.Compare(left, right);

        Assert.Equal(1, result);
    }

    [Fact]
    public void Compare_BothString_ReturnsCorrectComparison_Test()
    {
        var left = new Data<string>("apple");
        var right = new Data<string>("banana");

        var result = Comparator.Compare(left, right);

        Assert.True(result < 0);
    }

    [Fact]
    public void Compare_UnsupportedTypes_ThrowsException_Test()
    {
        var left = new Data<DateTime>(DateTime.Now);
        var right = new Data<Guid>(Guid.NewGuid());

        Assert.Throws<NotSupportedException>(() => Comparator.Compare(left, right));
    }
}