using System.Diagnostics;
using System.Globalization;
using System.Text;

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

public record PerformanceComparison(
    double Step,
    int ThreadsNumber,
    double SingleThreadMilliseconds,
    double MultithreadMilliseconds,
    double DifferenceMilliseconds,
    double SpeedupPercent);

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

    public static PerformanceComparison CompareWithSingleThread(
        double step,
        int threadsNumber,
        int repeats)
    {
        if (step <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(step));
        }

        if (threadsNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(threadsNumber));
        }

        if (repeats <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(repeats));
        }

        double singleThreadMilliseconds = MeasureAverageMilliseconds(repeats, () =>
        {
            DefiniteIntegral.SolveSingleThread(A, B, Math.Sin, step);
        });

        double multithreadMilliseconds = MeasureAverageMilliseconds(repeats, () =>
        {
            DefiniteIntegral.Solve(A, B, Math.Sin, step, threadsNumber);
        });

        double differenceMilliseconds = singleThreadMilliseconds - multithreadMilliseconds;
        double speedupPercent = differenceMilliseconds / singleThreadMilliseconds * 100.0;

        return new PerformanceComparison(
            step,
            threadsNumber,
            singleThreadMilliseconds,
            multithreadMilliseconds,
            differenceMilliseconds,
            speedupPercent);
    }

    public static string CreateReport(
        StepMeasurement selectedStep,
        ThreadMeasurement selectedThread,
        PerformanceComparison comparison,
        IEnumerable<StepMeasurement>? stepMeasurements = null,
        IEnumerable<ThreadMeasurement>? threadMeasurements = null)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Задание 15. Отчёт по производительности");
        builder.AppendLine();
        builder.AppendLine("Интеграл:");
        builder.AppendLine("Функция: sin(x)");
        builder.AppendLine("Отрезок: [-100, 100]");
        builder.AppendLine("Требуемая точность: 1e-4");
        builder.AppendLine();
        builder.AppendLine("Выбранный шаг:");
        builder.AppendLine($"Шаг: {FormatDouble(selectedStep.Step)}");
        builder.AppendLine($"Значение интеграла: {FormatDouble(selectedStep.Value)}");
        builder.AppendLine($"Погрешность: {FormatDouble(selectedStep.Error)}");
        builder.AppendLine($"Среднее время: {FormatDouble(selectedStep.AverageMilliseconds)} мс");
        builder.AppendLine();
        builder.AppendLine("Оптимальное количество потоков:");
        builder.AppendLine($"Количество потоков: {selectedThread.ThreadsNumber}");
        builder.AppendLine($"Значение интеграла: {FormatDouble(selectedThread.Value)}");
        builder.AppendLine($"Погрешность: {FormatDouble(selectedThread.Error)}");
        builder.AppendLine($"Среднее время: {FormatDouble(selectedThread.AverageMilliseconds)} мс");
        builder.AppendLine();
        builder.AppendLine("Сравнение однопоточной и многопоточной версии:");
        builder.AppendLine($"Шаг для сравнения: {FormatDouble(comparison.Step)}");
        builder.AppendLine($"Количество потоков в многопоточной версии: {comparison.ThreadsNumber}");
        builder.AppendLine($"Время однопоточной версии: {FormatDouble(comparison.SingleThreadMilliseconds)} мс");
        builder.AppendLine($"Время многопоточной версии: {FormatDouble(comparison.MultithreadMilliseconds)} мс");
        builder.AppendLine($"Разница: {FormatDouble(comparison.DifferenceMilliseconds)} мс");
        builder.AppendLine($"Ускорение: {FormatDouble(comparison.SpeedupPercent)}%");

        if (stepMeasurements is not null)
        {
            builder.AppendLine();
            builder.AppendLine("Замеры по шагам:");
            foreach (var measurement in stepMeasurements)
            {
                builder.AppendLine(
                    $"шаг={FormatDouble(measurement.Step)}, " +
                    $"значение={FormatDouble(measurement.Value)}, " +
                    $"погрешность={FormatDouble(measurement.Error)}, " +
                    $"среднееВремяМс={FormatDouble(measurement.AverageMilliseconds)}, " +
                    $"точностьДостигнута={measurement.IsAccurate}");
            }
        }

        if (threadMeasurements is not null)
        {
            builder.AppendLine();
            builder.AppendLine("Замеры по количеству потоков:");
            foreach (var measurement in threadMeasurements)
            {
                builder.AppendLine(
                    $"потоки={measurement.ThreadsNumber}, " +
                    $"значение={FormatDouble(measurement.Value)}, " +
                    $"погрешность={FormatDouble(measurement.Error)}, " +
                    $"среднееВремяМс={FormatDouble(measurement.AverageMilliseconds)}");
            }
        }

        return builder.ToString();
    }

    public static void SaveReport(
        string path,
        StepMeasurement selectedStep,
        ThreadMeasurement selectedThread,
        PerformanceComparison comparison,
        IEnumerable<StepMeasurement>? stepMeasurements = null,
        IEnumerable<ThreadMeasurement>? threadMeasurements = null)
    {
        var report = CreateReport(
            selectedStep,
            selectedThread,
            comparison,
            stepMeasurements,
            threadMeasurements);

        File.WriteAllText(path, report);
    }

    public static void SaveThreadMeasurementsCsv(
        string path,
        IEnumerable<ThreadMeasurement> threadMeasurements)
    {
        var builder = new StringBuilder();

        builder.AppendLine("threads,averageMilliseconds");

        foreach (var measurement in threadMeasurements)
        {
            builder.AppendLine(
                $"{measurement.ThreadsNumber}," +
                $"{FormatDouble(measurement.AverageMilliseconds)}");
        }

        File.WriteAllText(path, builder.ToString());
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

    private static string FormatDouble(double value)
    {
        return value.ToString("G17", CultureInfo.InvariantCulture);
    }
}
