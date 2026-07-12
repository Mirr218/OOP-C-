using System.Diagnostics;

namespace task15;

public record StepMeasurement(
    double Step,
    double Value,
    double Error,
    double AverageMilliseconds,
    bool IsAccurate);

public static class IntegralBenchmark
{
    public static readonly double[] DefaultSteps = [1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6];

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
