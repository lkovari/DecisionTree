using DecisionTreeLib.Data;

namespace DecisionTreeLib.Validators;

public class InvalidOperandTypeException : Exception
{
    public InvalidOperandTypeException(string message) : base(message) { }
    public InvalidOperandTypeException(string message, Exception innerException) : base(message, innerException) { }
}

public static class OperandTypeValidator
{
    public static void ValidateBitwiseOperands<TLeft, TRight>(IData<TLeft>? left, IData<TRight>? right)
    {
        if (left == null)
            throw new ArgumentNullException(nameof(left), "Left operand cannot be null");
        if (right == null)
            throw new ArgumentNullException(nameof(right), "Right operand cannot be null");
            
        if (!IsBitwiseCompatible(typeof(TLeft)) || !IsBitwiseCompatible(typeof(TRight)))
            throw new InvalidOperandTypeException($"Bitwise operations require byte, ushort, uint, or ulong types. Left: {typeof(TLeft)}, Right: {typeof(TRight)}");
    }

    public static void ValidateArithmeticOperands<TLeft, TRight>(IData<TLeft>? left, IData<TRight>? right)
    {
        if (left == null)
            throw new ArgumentNullException(nameof(left), "Left operand cannot be null");
        if (right == null)
            throw new ArgumentNullException(nameof(right), "Right operand cannot be null");
            
        if (!IsArithmeticCompatible(typeof(TLeft)) || !IsArithmeticCompatible(typeof(TRight)))
            throw new InvalidOperandTypeException($"Arithmetic operations require numeric types (byte, short, int, long, float, double, decimal, etc.). Left: {typeof(TLeft)}, Right: {typeof(TRight)}");
    }

    private static bool IsBitwiseCompatible(Type type) =>
        type == typeof(byte) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong);

    private static bool IsArithmeticCompatible(Type type) =>
        type == typeof(byte) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong) ||
        type == typeof(sbyte) || type == typeof(short) || type == typeof(int) || type == typeof(long) ||
        type == typeof(float) || type == typeof(double) || type == typeof(decimal);
}