
namespace DecisionTreeLib.Adapters;

public class ConsoleAdapter : IAdapter
{
    public void Write(string message) => Console.WriteLine(message);
}