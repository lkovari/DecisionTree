using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Request;

public interface IOperationRequest<TLeft, TRight> : IRequest<TLeft, TRight>
{
    OperatorType Operator { get; }
}