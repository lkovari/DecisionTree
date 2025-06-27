using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Request;

using DecisionTreeLib.Data;

public class OperationRequest<TLeft, TRight> : IOperationRequest<TLeft, TRight>
{
    public IData<TLeft> LeftOperand { get; set; }
    public IData<TRight> RightOperand { get; set; }
    public OperatorType Operator { get; }

    public OperationRequest(IData<TLeft> leftOperand, IData<TRight> rightOperand, OperatorType operatorType)
    {
        LeftOperand = leftOperand;
        RightOperand = rightOperand;
        Operator = operatorType;
    }
}