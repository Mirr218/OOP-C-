namespace task14;
using System.Threading;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        Thread[] threads = new Thread[threadsNumber];
        
        double result = 0.0;
        
        using var barrier = new Barrier(threadsNumber + 1);

        double partLength = (b - a) / threadsNumber;

        for (int i = 0; i < threadsNumber; i++)
        {
            double localA = a + i * partLength;
            double localB = i == threadsNumber - 1 ? b : localA + partLength;

            threads[i] = new Thread(() =>
            {
                double localResult = CalculatePart(localA, localB, function, step);
                AddToResult(ref result, localResult);
                barrier.SignalAndWait();
            });

            threads[i].Start();
        }

        barrier.SignalAndWait();

        return result;
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

    private static void AddToResult(ref double result, double value)
    {
        double initialValue;
        double computedValue;

        do
        {
            initialValue = result;
            computedValue = initialValue + value;
        }
        while (Interlocked.CompareExchange(ref result, computedValue, initialValue) != initialValue);
    }
}
