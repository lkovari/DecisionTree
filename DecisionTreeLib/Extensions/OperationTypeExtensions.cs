using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Extensions;

public static class OperatorTypeExtensions
{
    public static string ToSymbol(this OperatorType op)
    {
        return op switch
        {
            OperatorType.Add => "+",
            OperatorType.Subtract => "-",
            OperatorType.Multiply => "*",
            OperatorType.Divide => "/",
            OperatorType.And => "&&",
            OperatorType.Or => "||",
            OperatorType.Not => "!",
            OperatorType.Xor => "^",
            OperatorType.Nand => "!&&",
            OperatorType.Nor => "!||",
            _ => op.ToString()
        };
    }
}