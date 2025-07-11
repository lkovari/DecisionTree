namespace DecisionTreeLib.Node.Operations;

public interface IOperationStrategy
{
    object Calculate(object left, object right);
} 