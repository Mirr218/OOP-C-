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
}
