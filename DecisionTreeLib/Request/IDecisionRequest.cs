using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Request;

public interface IDecisionRequest<TLeft, TRight> : IRequest<TLeft, TRight>
{
    RelationType Relation { get; }
}