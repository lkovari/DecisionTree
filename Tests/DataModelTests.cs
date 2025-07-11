using DecisionTreeLib.Data;
using Xunit;

namespace Tests;

public class DataModelTests
{
    [Fact]
    public void Data_StoresAndReturnsValue_Int()
    {
        IData<int> data = new Data<int>(42);
        Assert.Equal(42, data.Value);

        data.Value = 100;
        Assert.Equal(100, data.Value);
    }

    [Fact]
    public void Data_StoresAndReturnsValue_String()
    {
        IData<string> data = new Data<string>("hello");
        Assert.Equal("hello", data.Value);

        data.Value = "world";
        Assert.Equal("world", data.Value);
    }

    [Fact]
    public void Data_StoresAndReturnsValue_Nullable()
    {
        IData<int?> data = new Data<int?>(null);
        Assert.Null(data.Value);

        data.Value = 5;
        Assert.Equal(5, data.Value);
    }

    [Fact]
    public void Data_DefaultValue_IsNullOrDefault()
    {
        IData<string> data = new Data<string>(default);
        Assert.Null(data.Value);

        IData<int> dataInt = new Data<int>(default);
        Assert.Equal(0, dataInt.Value);
    }

    [Fact]
    public void Data_CanBeUsedAsIData()
    {
        IData<double> data = new Data<double>(3.14);
        Assert.IsAssignableFrom<IData<double>>(data);
        Assert.Equal(3.14, data.Value);
    }
}