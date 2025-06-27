using DecisionTreeLib.Enums;
using DecisionTreeLib.Data;

namespace DecisionTreeLib.Request;

public class DecisionRequest<TLeft, TRight> : IDecisionRequest<TLeft, TRight>
{
    public IData<TLeft> LeftOperand { get; set; }
    public IData<TRight> RightOperand { get; set; }
    public RelationType Relation { get; }

    public DecisionRequest(IData<TLeft> leftOperand, IData<TRight> rightOperand, RelationType relation)
    {
        LeftOperand = leftOperand;
        RightOperand = rightOperand;
        Relation = relation;
    }
}