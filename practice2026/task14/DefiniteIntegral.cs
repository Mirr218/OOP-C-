namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
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
