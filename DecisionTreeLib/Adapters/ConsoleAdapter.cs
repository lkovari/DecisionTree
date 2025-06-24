using DecisionTreeLib.Data;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Adapters;

public class ConsoleAdapter<T> : IAdapter<T>
{
    public void Write(string message) => Console.WriteLine(message);
}