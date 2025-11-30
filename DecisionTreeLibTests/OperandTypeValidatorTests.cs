using DecisionTreeLib.Data;
using DecisionTreeLib.Validators;

namespace DecisionTreeLibTests;

public class OperandTypeValidatorTests
{
    [Fact]
    public void ValidateBitwiseOperands_WithValidTypes_DoesNotThrow()
    {
        var left = new Data<byte>(5);
        var right = new Data<byte>(3);
        
        OperandTypeValidator.ValidateBitwiseOperands(left, right);
    }

    [Fact]
    public void ValidateBitwiseOperands_WithNullLeft_ThrowsArgumentNullException()
    {
        var right = new Data<byte>(3);
        
        Assert.Throws<ArgumentNullException>(() => 
            OperandTypeValidator.ValidateBitwiseOperands<byte, byte>(null, right));
    }

    [Fact]
    public void ValidateBitwiseOperands_WithNullRight_ThrowsArgumentNullException()
    {
        var left = new Data<byte>(5);
        
        Assert.Throws<ArgumentNullException>(() => 
            OperandTypeValidator.ValidateBitwiseOperands<byte, byte>(left, null));
    }

    [Fact]
    public void ValidateBitwiseOperands_WithInvalidTypes_ThrowsInvalidOperandTypeException()
    {
        var left = new Data<float>(5.5f);
        var right = new Data<double>(3.3);
        
        Assert.Throws<InvalidOperandTypeException>(() => 
            OperandTypeValidator.ValidateBitwiseOperands(left, right));
    }

    [Theory]
    [InlineData(typeof(byte), typeof(byte))]
    [InlineData(typeof(ushort), typeof(ushort))]
    [InlineData(typeof(uint), typeof(uint))]
    [InlineData(typeof(ulong), typeof(ulong))]
    [InlineData(typeof(sbyte), typeof(sbyte))]
    [InlineData(typeof(short), typeof(short))]
    [InlineData(typeof(int), typeof(int))]
    [InlineData(typeof(long), typeof(long))]
    public void ValidateBitwiseOperands_WithAllValidIntegerTypes_DoesNotThrow(Type leftType, Type rightType)
    {
        var left = Activator.CreateInstance(typeof(Data<>).MakeGenericType(leftType), Convert.ChangeType(5, leftType));
        var right = Activator.CreateInstance(typeof(Data<>).MakeGenericType(rightType), Convert.ChangeType(3, rightType));
        
        var method = typeof(OperandTypeValidator).GetMethod("ValidateBitwiseOperands")!
            .MakeGenericMethod(leftType, rightType);
        
        method.Invoke(null, new[] { left, right });
    }

    [Fact]
    public void ValidateArithmeticOperands_WithValidTypes_DoesNotThrow()
    {
        var left = new Data<int>(5);
        var right = new Data<int>(3);
        
        OperandTypeValidator.ValidateArithmeticOperands(left, right);
    }

    [Fact]
    public void ValidateArithmeticOperands_WithNullLeft_ThrowsArgumentNullException()
    {
        var right = new Data<int>(3);
        
        Assert.Throws<ArgumentNullException>(() => 
            OperandTypeValidator.ValidateArithmeticOperands<int, int>(null, right));
    }

    [Fact]
    public void ValidateArithmeticOperands_WithNullRight_ThrowsArgumentNullException()
    {
        var left = new Data<int>(5);
        
        Assert.Throws<ArgumentNullException>(() => 
            OperandTypeValidator.ValidateArithmeticOperands<int, int>(left, null));
    }

    [Fact]
    public void ValidateArithmeticOperands_WithStringTypes_ThrowsInvalidOperandTypeException()
    {
        var left = new Data<string>("5");
        var right = new Data<string>("3");
        
        Assert.Throws<InvalidOperandTypeException>(() => 
            OperandTypeValidator.ValidateArithmeticOperands(left, right));
    }

    [Theory]
    [InlineData(typeof(byte), typeof(byte))]
    [InlineData(typeof(ushort), typeof(ushort))]
    [InlineData(typeof(uint), typeof(uint))]
    [InlineData(typeof(ulong), typeof(ulong))]
    [InlineData(typeof(sbyte), typeof(sbyte))]
    [InlineData(typeof(short), typeof(short))]
    [InlineData(typeof(int), typeof(int))]
    [InlineData(typeof(long), typeof(long))]
    [InlineData(typeof(float), typeof(float))]
    [InlineData(typeof(double), typeof(double))]
    [InlineData(typeof(decimal), typeof(decimal))]
    public void ValidateArithmeticOperands_WithAllValidNumericTypes_DoesNotThrow(Type leftType, Type rightType)
    {
        var left = Activator.CreateInstance(typeof(Data<>).MakeGenericType(leftType), Convert.ChangeType(5, leftType));
        var right = Activator.CreateInstance(typeof(Data<>).MakeGenericType(rightType), Convert.ChangeType(3, rightType));
        
        var method = typeof(OperandTypeValidator).GetMethod("ValidateArithmeticOperands")!
            .MakeGenericMethod(leftType, rightType);
        
        method.Invoke(null, new[] { left, right });
    }
}

