using DecisionTreeLib.Data;

namespace DecisionTreeLib.Request;

public interface IRequest<TLeft, TRight> 
{
    IData<TLeft> LeftOperand { get; set; }
    IData<TRight> RightOperand { get; set; }
}