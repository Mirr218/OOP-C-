using task15;

namespace task15tests;

public class DefiniteIntegralTests
{
    [Fact]
    public void Solve_ForSinFromMinus100To100_ReturnsZero()
    {
        Func<double, double> function = x => Math.Sin(x);

        var result = DefiniteIntegral.Solve(-100, 100, function, 1e-4, 8);

        Assert.Equal(0, result, 1e-4);
    }

    [Fact]
    public void SolveSingleThread_ForSinFromMinus100To100_ReturnsZero()
    {
        Func<double, double> function = x => Math.Sin(x);

        var result = DefiniteIntegral.SolveSingleThread(-100, 100, function, 1e-4);

        Assert.Equal(0, result, 1e-4);
    }

    [Fact]
    public void SolveAndSolveSingleThread_ReturnCloseResults()
    {
        Func<double, double> function = x => x;

        var multithreaded = DefiniteIntegral.Solve(0, 5, function, 1e-5, 4);
        var singleThreaded = DefiniteIntegral.SolveSingleThread(0, 5, function, 1e-5);

        Assert.Equal(singleThreaded, multithreaded, 1e-4);
    }
}
