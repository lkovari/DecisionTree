namespace DecisionTreeLib.Helper;

public static class ExpressionTextFormatHelper
{
    public static string FormatOperation(object leftOperand, string operation, object rightOperand, object result)
    {
        return $"{leftOperand} {operation} {rightOperand} = {result}";
    }

    public static string FormatRelation(object leftOperand, string relation, object rightOperand, bool condition)
    {
        return $"{leftOperand} {relation} {rightOperand} = {condition}";
    }
}