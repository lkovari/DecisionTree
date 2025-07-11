using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Request;

public interface IBinaryRequest<TLeft, TRight> : IRequest<TLeft, TRight>
{
    IData<TLeft> LeftOperand { get; set; }
    IData<TRight> RightOperand { get; set; }
    OperatorType Operator { get; set; }
} 