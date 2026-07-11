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
}
