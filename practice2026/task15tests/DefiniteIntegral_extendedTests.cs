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

    [Fact]
    public void MeasureSteps_ReturnsMeasurementsForEachStep()
    {
        double[] steps = [1e-1, 1e-2];

        var measurements = IntegralBenchmark.MeasureSteps(threadsNumber: 2, repeats: 1, steps);

        Assert.Equal(2, measurements.Count);
        Assert.All(measurements, measurement =>
        {
            Assert.True(measurement.Step > 0);
            Assert.True(measurement.AverageMilliseconds >= 0);
            Assert.True(measurement.Error >= 0);
        });
    }

    [Fact]
    public void FindFastestAccurateStep_ReturnsAccurateMeasurement()
    {
        double[] steps = [1e-1, 1e-2];

        var measurement = IntegralBenchmark.FindFastestAccurateStep(threadsNumber: 2, repeats: 1, steps);

        Assert.True(measurement.IsAccurate);
        Assert.True(measurement.Error <= 1e-4);
    }

    [Fact]
    public void MeasureSteps_WithInvalidRepeats_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            IntegralBenchmark.MeasureSteps(threadsNumber: 2, repeats: 0, steps: [1e-1]));
    }

    [Fact]
    public void MeasureThreads_ReturnsMeasurementsForEachThreadCount()
    {
        int[] threadCounts = [1, 2];

        var measurements = IntegralBenchmark.MeasureThreads(step: 1e-2, repeats: 1, threadCounts);

        Assert.Equal(2, measurements.Count);
        Assert.All(measurements, measurement =>
        {
            Assert.True(measurement.ThreadsNumber > 0);
            Assert.True(measurement.AverageMilliseconds >= 0);
            Assert.True(measurement.Error >= 0);
        });
    }

    [Fact]
    public void FindFastestThreadCount_ReturnsMeasurementFromThreadCounts()
    {
        int[] threadCounts = [1, 2];

        var measurement = IntegralBenchmark.FindFastestThreadCount(step: 1e-2, repeats: 1, threadCounts);

        Assert.Contains(measurement.ThreadsNumber, threadCounts);
    }

    [Fact]
    public void MeasureThreads_WithInvalidStep_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            IntegralBenchmark.MeasureThreads(step: 0, repeats: 1, threadCounts: [1, 2]));
    }

    [Fact]
    public void CompareWithSingleThread_ReturnsPerformanceComparison()
    {
        var comparison = IntegralBenchmark.CompareWithSingleThread(
            step: 1e-2,
            threadsNumber: 2,
            repeats: 1);

        Assert.Equal(1e-2, comparison.Step);
        Assert.Equal(2, comparison.ThreadsNumber);
        Assert.True(comparison.SingleThreadMilliseconds >= 0);
        Assert.True(comparison.MultithreadMilliseconds >= 0);
    }
}
