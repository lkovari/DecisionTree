namespace DecisionTreeLib.Node.Operations;

public class DivideOperation : IOperationStrategy
{
    // WARNING: Using dynamic disables compile-time type safety. Only use for numeric types.
    public object Calculate(object left, object right)
    {
        if ((dynamic)right == 0)
            throw new DivideByZeroException();
        return (dynamic)left / (dynamic)right;
    }
}