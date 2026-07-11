using task14;
using Xunit;

namespace task14tests;

public class DefiniteIntegralTests
{
    [Fact]
    public void Solve_ForLinearFunctionOnSymmetricSegment_ReturnsZero()
    {
        Func<double, double> function = x => x;

        var result = DefiniteIntegral.Solve(-1, 1, function, 1e-4, 2);

        Assert.Equal(0, result, 1e-4);
    }

    [Fact]
    public void Solve_ForLinearFunctionFromZeroToFive_ReturnsTwelvePointFive()
    {
        Func<double, double> function = x => x;

        var result = DefiniteIntegral.Solve(0, 5, function, 1e-6, 8);

        Assert.Equal(12.5, result, 1e-5);
    }

    [Fact]
    public void Solve_ForSinFunctionOnSymmetricSegment_ReturnsZero()
    {
        Func<double, double> function = x => Math.Sin(x);

        var result = DefiniteIntegral.Solve(-1, 1, function, 1e-5, 8);

        Assert.Equal(0, result, 1e-4);
    }

    [Fact]
    public void Solve_ForConstantFunction_ReturnsRectangleArea()
    {
        Func<double, double> function = _ => 2;

        var result = DefiniteIntegral.Solve(0, 3, function, 1e-4, 4);

        Assert.Equal(6, result, 1e-4);
    }

    [Fact]
    public void Solve_WithSingleThread_ReturnsCorrectResult()
    {
        Func<double, double> function = x => x;

        var result = DefiniteIntegral.Solve(0, 2, function, 1e-5, 1);

        Assert.Equal(2, result, 1e-4);
    }

    [Fact]
    public void Solve_WithNullFunction_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => DefiniteIntegral.Solve(0, 1, null!, 1e-4, 2));
    }

    [Fact]
    public void Solve_WithZeroStep_ThrowsException()
    {
        Func<double, double> function = x => x;

        Assert.Throws<ArgumentOutOfRangeException>(() => DefiniteIntegral.Solve(0, 1, function, 0, 2));
    }

    [Fact]
    public void Solve_WithNegativeThreadsNumber_ThrowsException()
    {
        Func<double, double> function = x => x;

        Assert.Throws<ArgumentOutOfRangeException>(() => DefiniteIntegral.Solve(0, 1, function, 1e-4, -1));
    }
}
