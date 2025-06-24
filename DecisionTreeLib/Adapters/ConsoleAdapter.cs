using DecisionTreeLib.Data;

namespace DecisionTreeLib.Adapters;

public class ConsoleAdapter<T> : IAdapter<T>
{
    public void Show(IData<T> data) => Console.WriteLine($"Log result: {data.Value}");
}