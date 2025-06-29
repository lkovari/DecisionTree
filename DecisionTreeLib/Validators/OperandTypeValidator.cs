using DecisionTreeLib.Data;

namespace DecisionTreeLib.Validators;

public static class OperandTypeValidator
{
    public static void ValidateBitwiseOperands<TLeft, TRight>(IData<TLeft> left, IData<TRight> right)
    {
        if (!IsBitwiseCompatible(typeof(TLeft)) || !IsBitwiseCompatible(typeof(TRight)))
            throw new InvalidOperationException($"Bitwise operations require byte, ushort, uint, or ulong types. Left: {typeof(TLeft)}, Right: {typeof(TRight)}");
    }

    public static void ValidateArithmeticOperands<TLeft, TRight>(IData<TLeft> left, IData<TRight> right)
    {
        if (!IsArithmeticCompatible(typeof(TLeft)) || !IsArithmeticCompatible(typeof(TRight)))
            throw new InvalidOperationException($"Arithmetic operations require numeric types (byte, short, int, long, float, double, decimal, etc.). Left: {typeof(TLeft)}, Right: {typeof(TRight)}");
    }

    private static bool IsBitwiseCompatible(Type type) =>
        type == typeof(byte) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong);

    private static bool IsArithmeticCompatible(Type type) =>
        type == typeof(byte) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong) ||
        type == typeof(sbyte) || type == typeof(short) || type == typeof(int) || type == typeof(long) ||
        type == typeof(float) || type == typeof(double) || type == typeof(decimal);
}