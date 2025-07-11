using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Request;

public class UnaryOperationRequest<TOperand> : IUnaryRequest<TOperand>
{
    public IData<TOperand> Operand { get; set; }
    public OperatorType Operator { get; set; }
} 