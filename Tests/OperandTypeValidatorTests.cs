using DecisionTreeLib.Data;
using DecisionTreeLib.Validators;
using Xunit;
using System;

namespace Tests;

public class OperandTypeValidatorTests
{
    [Fact]
    public void ValidateBitwiseOperands_ValidTypes_DoesNotThrow()
    {
        var left = new Data<byte>(1);
        var right = new Data<ushort>(2);
        OperandTypeValidator.ValidateBitwiseOperands(left, right);
        OperandTypeValidator.ValidateBitwiseOperands(new Data<uint>(1), new Data<ulong>(2));
    }

    [Fact]
    public void ValidateBitwiseOperands_InvalidLeftType_Throws()
    {
        var left = new Data<int>(1);
        var right = new Data<byte>(2);
        Assert.Throws<InvalidOperandTypeException>(() =>
            OperandTypeValidator.ValidateBitwiseOperands(left, right));
    }

    [Fact]
    public void ValidateBitwiseOperands_InvalidRightType_Throws()
    {
        var left = new Data<byte>(1);
        var right = new Data<int>(2);
        Assert.Throws<InvalidOperandTypeException>(() =>
            OperandTypeValidator.ValidateBitwiseOperands(left, right));
    }

    [Fact]
    public void ValidateArithmeticOperands_ValidTypes_DoesNotThrow()
    {
        var left = new Data<int>(1);
        var right = new Data<decimal>(2);
        OperandTypeValidator.ValidateArithmeticOperands(left, right);
        OperandTypeValidator.ValidateArithmeticOperands(new Data<float>(1), new Data<double>(2));
    }

    [Fact]
    public void ValidateArithmeticOperands_InvalidLeftType_Throws()
    {
        var left = new Data<string>("x");
        var right = new Data<int>(2);
        Assert.Throws<InvalidOperandTypeException>(() =>
            OperandTypeValidator.ValidateArithmeticOperands(left, right));
    }

    [Fact]
    public void ValidateArithmeticOperands_InvalidRightType_Throws()
    {
        var left = new Data<int>(1);
        var right = new Data<string>("x");
        Assert.Throws<InvalidOperandTypeException>(() =>
            OperandTypeValidator.ValidateArithmeticOperands(left, right));
    }

    // Null operand tests
    [Fact]
    public void ValidateBitwiseOperands_NullLeft_ThrowsArgumentNullException()
    {
        IData<byte>? left = null;
        var right = new Data<byte>(1);
        Assert.Throws<ArgumentNullException>(() => OperandTypeValidator.ValidateBitwiseOperands(left, right));
    }

    [Fact]
    public void ValidateBitwiseOperands_NullRight_ThrowsArgumentNullException()
    {
        var left = new Data<byte>(1);
        IData<byte>? right = null;
        Assert.Throws<ArgumentNullException>(() => OperandTypeValidator.ValidateBitwiseOperands(left, right));
    }

    [Fact]
    public void ValidateArithmeticOperands_NullLeft_ThrowsArgumentNullException()
    {
        IData<int>? left = null;
        var right = new Data<int>(1);
        Assert.Throws<ArgumentNullException>(() => OperandTypeValidator.ValidateArithmeticOperands(left, right));
    }

    [Fact]
    public void ValidateArithmeticOperands_NullRight_ThrowsArgumentNullException()
    {
        var left = new Data<int>(1);
        IData<int>? right = null;
        Assert.Throws<ArgumentNullException>(() => OperandTypeValidator.ValidateArithmeticOperands(left, right));
    }

    // All supported types for bitwise
    [Theory]
    [InlineData(typeof(byte))]
    [InlineData(typeof(ushort))]
    [InlineData(typeof(uint))]
    [InlineData(typeof(ulong))]
    public void ValidateBitwiseOperands_SupportedTypes_DoesNotThrow(Type type)
    {
        var method = typeof(OperandTypeValidatorTests).GetMethod(nameof(ValidateBitwiseOperands_SupportedTypeHelper), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;
        var generic = method.MakeGenericMethod(type);
        generic.Invoke(null, null);
    }

    private static void ValidateBitwiseOperands_SupportedTypeHelper<T>()
    {
        var left = new Data<T>((T)Convert.ChangeType(1, typeof(T)));
        var right = new Data<T>((T)Convert.ChangeType(2, typeof(T)));
        OperandTypeValidator.ValidateBitwiseOperands(left, right);
    }

    // All supported types for arithmetic
    [Theory]
    [InlineData(typeof(byte))]
    [InlineData(typeof(ushort))]
    [InlineData(typeof(uint))]
    [InlineData(typeof(ulong))]
    [InlineData(typeof(sbyte))]
    [InlineData(typeof(short))]
    [InlineData(typeof(int))]
    [InlineData(typeof(long))]
    [InlineData(typeof(float))]
    [InlineData(typeof(double))]
    [InlineData(typeof(decimal))]
    public void ValidateArithmeticOperands_SupportedTypes_DoesNotThrow(Type type)
    {
        var method = typeof(OperandTypeValidatorTests).GetMethod(nameof(ValidateArithmeticOperands_SupportedTypeHelper), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;
        var generic = method.MakeGenericMethod(type);
        generic.Invoke(null, null);
    }

    private static void ValidateArithmeticOperands_SupportedTypeHelper<T>()
    {
        var left = new Data<T>((T)Convert.ChangeType(1, typeof(T)));
        var right = new Data<T>((T)Convert.ChangeType(2, typeof(T)));
        OperandTypeValidator.ValidateArithmeticOperands(left, right);
    }

    // Unsupported type for bitwise
    [Fact]
    public void ValidateBitwiseOperands_UnsupportedType_Throws()
    {
        var left = new Data<string>("a");
        var right = new Data<string>("b");
        Assert.Throws<InvalidOperandTypeException>(() => OperandTypeValidator.ValidateBitwiseOperands(left, right));
    }

    // Unsupported type for arithmetic
    [Fact]
    public void ValidateArithmeticOperands_UnsupportedType_Throws()
    {
        var left = new Data<string>("a");
        var right = new Data<string>("b");
        Assert.Throws<InvalidOperandTypeException>(() => OperandTypeValidator.ValidateArithmeticOperands(left, right));
    }
}