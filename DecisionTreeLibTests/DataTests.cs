using DecisionTreeLib.Data;

namespace DecisionTreeLibTests;

public class DataTests
{
    [Fact]
    public void Data_Constructor_SetsValue()
    {
        var data = new Data<int>(42);
        
        Assert.Equal(42, data.Value);
    }

    [Fact]
    public void Data_Value_CanBeSet()
    {
        var data = new Data<int>(42);
        data.Value = 100;
        
        Assert.Equal(100, data.Value);
    }

    [Fact]
    public void Data_WithNullValue_StoresNull()
    {
        var data = new Data<string?>(null);
        
        Assert.Null(data.Value);
    }

    [Fact]
    public void Data_WithReferenceType_StoresValue()
    {
        var obj = new object();
        var data = new Data<object>(obj);
        
        Assert.Equal(obj, data.Value);
    }

    [Fact]
    public void Data_WithValueType_StoresValue()
    {
        var data = new Data<decimal>(123.45m);
        
        Assert.Equal(123.45m, data.Value);
    }
}

