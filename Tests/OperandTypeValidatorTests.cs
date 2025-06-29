
using DecisionTreeLib.Data;
using DecisionTreeLib.Validators;

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
        Assert.Throws<InvalidOperationException>(() =>
            OperandTypeValidator.ValidateBitwiseOperands(left, right));
    }

    [Fact]
    public void ValidateBitwiseOperands_InvalidRightType_Throws()
    {
        var left = new Data<byte>(1);
        var right = new Data<int>(2);
        Assert.Throws<InvalidOperationException>(() =>
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
        Assert.Throws<InvalidOperationException>(() =>
            OperandTypeValidator.ValidateArithmeticOperands(left, right));
    }

    [Fact]
    public void ValidateArithmeticOperands_InvalidRightType_Throws()
    {
        var left = new Data<int>(1);
        var right = new Data<string>("x");
        Assert.Throws<InvalidOperationException>(() =>
            OperandTypeValidator.ValidateArithmeticOperands(left, right));
    }
}