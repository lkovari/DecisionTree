

namespace DecisionTreeLib.Node.Operations;

public class AndOperation : IOperationStrategy
{
    // WARNING: Using dynamic disables compile-time type safety. Only use for numeric types.
    public object Calculate(object left, object right)
    {
        return (dynamic)left & (dynamic)right;
    }
} 