using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Request;

public interface IUnaryRequest<TOperand>
{
    IData<TOperand> Operand { get; set; }
    OperatorType Operator { get; set; }
} 