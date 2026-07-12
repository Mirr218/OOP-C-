using System.Threading;

namespace task15;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        ValidateArguments(function, step, threadsNumber);

        Thread[] threads = new Thread[threadsNumber];
        double[] partialResults = new double[threadsNumber];
        using var barrier = new Barrier(threadsNumber + 1);

        double partLength = (b - a) / threadsNumber;

        for (int i = 0; i < threadsNumber; i++)
        {
            double localA = a + i * partLength;
            double localB = i == threadsNumber - 1 ? b : localA + partLength;
            int threadIndex = i;

            threads[i] = new Thread(() =>
            {
                partialResults[threadIndex] = CalculatePart(localA, localB, function, step);
                barrier.SignalAndWait();
            });

            threads[i].Start();
        }

        barrier.SignalAndWait();

        return partialResults.Sum();
    }

    public static double SolveSingleThread(double a, double b, Func<double, double> function, double step)
    {
        ValidateArguments(function, step, threadsNumber: 1);

        return CalculatePart(a, b, function, step);
    }

    private static void ValidateArguments(Func<double, double>? function, double step, int threadsNumber)
    {
        if (function is null)
        {
            throw new ArgumentNullException(nameof(function));
        }

        if (step <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(step));
        }

        if (threadsNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(threadsNumber));
        }
    }

    private static double CalculatePart(double a, double b, Func<double, double> function, double step)
    {
        double result = 0.0;

        for (double x = a; x < b; x += step)
        {
            double nextX = Math.Min(x + step, b);
            double heightLeft = function(x);
            double heightRight = function(nextX);

            result += (heightLeft + heightRight) / 2.0 * (nextX - x);
        }

        return result;
    }
}
