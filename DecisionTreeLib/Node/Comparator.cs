using DecisionTreeLib.Data;

namespace DecisionTreeLib.Node;

public static class Comparator
{
    public static int Compare<TLeft, TRight>(IData<TLeft> left, IData<TRight> right)
    {
        object? leftValue = left.Value;
        object? rightValue = right.Value;

        if (leftValue is null && rightValue is null)
            return 0;
        if (leftValue is null)
            return -1;
        if (rightValue is null)
            return 1;

        Type leftType = leftValue.GetType();
        Type rightType = rightValue.GetType();

        // Numeric comparison
        if (IsNumeric(leftType) && IsNumeric(rightType))
        {
            double leftNum = Convert.ToDouble(leftValue);
            double rightNum = Convert.ToDouble(rightValue);
            return leftNum.CompareTo(rightNum);
        }

        // String comparison
        if (leftValue is string leftStr && rightValue is string rightStr)
        {
            return string.Compare(leftStr, rightStr, StringComparison.Ordinal);
        }

        throw new NotSupportedException($"Unsupported comparison between types {leftType.Name} and {rightType.Name}.");
    }

    private static bool IsNumeric(Type type)
    {
        return type == typeof(byte) || type == typeof(sbyte) ||
               type == typeof(short) || type == typeof(ushort) ||
               type == typeof(int) || type == typeof(uint) ||
               type == typeof(long) || type == typeof(ulong) ||
               type == typeof(float) || type == typeof(double) ||
               type == typeof(decimal);
    }
}
