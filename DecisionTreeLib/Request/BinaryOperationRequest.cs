using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Request;

public class BinaryOperationRequest<TLeft, TRight> : IBinaryRequest<TLeft, TRight>
{
    public IData<TLeft> LeftOperand { get; set; }
    public IData<TRight> RightOperand { get; set; }
    public OperatorType Operator { get; set; }
} 