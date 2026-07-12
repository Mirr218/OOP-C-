using System.Diagnostics;

namespace task15;

public record StepMeasurement(
    double Step,
    double Value,
    double Error,
    double AverageMilliseconds,
    bool IsAccurate);

public record ThreadMeasurement(
    int ThreadsNumber,
    double Value,
    double Error,
    double AverageMilliseconds);

public static class IntegralBenchmark
{
    public static readonly double[] DefaultSteps = [1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6];
    public static readonly int[] DefaultThreadCounts = [1, 2, 4, 8, 16];

    private const double A = -100;
    private const double B = 100;
    private const double ExpectedValue = 0.0;
    private const double RequiredAccuracy = 1e-4;

    public static IReadOnlyList<StepMeasurement> MeasureSteps(
        int threadsNumber,
        int repeats,
        IEnumerable<double>? steps = null)
    {
        if (threadsNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(threadsNumber));
        }

        if (repeats <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(repeats));
        }

        var selectedSteps = steps ?? DefaultSteps;
        var results = new List<StepMeasurement>();

        foreach (double step in selectedSteps)
        {
            double value = 0.0;
            double averageMilliseconds = MeasureAverageMilliseconds(repeats, () =>
            {
                value = DefiniteIntegral.Solve(A, B, Math.Sin, step, threadsNumber);
            });

            double error = Math.Abs(value - ExpectedValue);

            results.Add(new StepMeasurement(
                step,
                value,
                error,
                averageMilliseconds,
                error <= RequiredAccuracy));
        }

        return results;
    }

    public static StepMeasurement FindFastestAccurateStep(
        int threadsNumber,
        int repeats,
        IEnumerable<double>? steps = null)
    {
        return MeasureSteps(threadsNumber, repeats, steps)
            .Where(measurement => measurement.IsAccurate)
            .OrderBy(measurement => measurement.AverageMilliseconds)
            .First();
    }

    public static IReadOnlyList<ThreadMeasurement> MeasureThreads(
        double step,
        int repeats,
        IEnumerable<int>? threadCounts = null)
    {
        if (step <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(step));
        }

        if (repeats <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(repeats));
        }

        var selectedThreadCounts = threadCounts ?? DefaultThreadCounts;
        var results = new List<ThreadMeasurement>();

        foreach (int threadsNumber in selectedThreadCounts)
        {
            if (threadsNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(threadCounts));
            }

            double value = 0.0;
            double averageMilliseconds = MeasureAverageMilliseconds(repeats, () =>
            {
                value = DefiniteIntegral.Solve(A, B, Math.Sin, step, threadsNumber);
            });

            double error = Math.Abs(value - ExpectedValue);

            results.Add(new ThreadMeasurement(
                threadsNumber,
                value,
                error,
                averageMilliseconds));
        }

        return results;
    }

    public static ThreadMeasurement FindFastestThreadCount(
        double step,
        int repeats,
        IEnumerable<int>? threadCounts = null)
    {
        return MeasureThreads(step, repeats, threadCounts)
            .OrderBy(measurement => measurement.AverageMilliseconds)
            .First();
    }

    private static double MeasureAverageMilliseconds(int repeats, Action action)
    {
        double totalMilliseconds = 0.0;

        for (int i = 0; i < repeats; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();

            totalMilliseconds += stopwatch.Elapsed.TotalMilliseconds;
        }

        return totalMilliseconds / repeats;
    }
}
