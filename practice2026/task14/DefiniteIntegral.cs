namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        double result = 0.0;
        
        double partLength = (b - a) / threadsNumber;

        for (int i = 0; i < threadsNumber; i++)
        {
            double localA = a + i * partLength;
            double localB = i == threadsNumber - 1 ? b : localA + partLength;

            result += CalculatePart(localA, localB, function, step);
        }

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
}
