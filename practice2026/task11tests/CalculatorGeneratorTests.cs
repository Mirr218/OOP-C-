using task11;
using Xunit;

namespace task11tests;

public class CalculatorGeneratorTests
{
    private const string CalculatorCode = @"
using System;

public class Calculator : ICalculator
{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
}";

    [Fact]
    public void Generate_ShouldCreateInstance()
    {
        // Act
        var calculator = CalculatorGenerator.Generate<ICalculator>(CalculatorCode, "Calculator");

        // Assert
        Assert.NotNull(calculator);
    }

    [Fact]
    public void Generate_Add_ShouldReturnSum()
    {
        // Arrange
        var calculator = CalculatorGenerator.Generate<ICalculator>(CalculatorCode, "Calculator");

        // Act
        var result = calculator.Add(5, 3);

        // Assert
        Assert.Equal(8, result);
    }

    [Fact]
    public void Generate_Minus_ShouldReturnDifference()
    {
        // Arrange
        var calculator = CalculatorGenerator.Generate<ICalculator>(CalculatorCode, "Calculator");

        // Act
        var result = calculator.Minus(10, 4);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Generate_Mul_ShouldReturnProduct()
    {
        // Arrange
        var calculator = CalculatorGenerator.Generate<ICalculator>(CalculatorCode, "Calculator");

        // Act
        var result = calculator.Mul(3, 7);

        // Assert
        Assert.Equal(21, result);
    }

    [Fact]
    public void Generate_Div_ShouldReturnQuotient()
    {
        // Arrange
        var calculator = CalculatorGenerator.Generate<ICalculator>(CalculatorCode, "Calculator");

        // Act
        var result = calculator.Div(20, 4);

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Generate_WithInvalidCode_ShouldThrowException()
    {
        // Arrange
        string invalidCode = @"
public class InvalidClass
{
    public void InvalidMethod(
}";

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            CalculatorGenerator.Generate<ICalculator>(invalidCode, "InvalidClass"));
    }

    [Fact]
    public void Generate_WithTypeNotFound_ShouldThrowException()
    {
        // Arrange
        string codeWithoutTargetType = @"
public class SomeOtherClass
{
    public void DoSomething() { }
}";

        // Act & Assert
        Assert.Throws<TypeLoadException>(() => 
            CalculatorGenerator.Generate<ICalculator>(codeWithoutTargetType, "NonExistentClass"));
    }
}
