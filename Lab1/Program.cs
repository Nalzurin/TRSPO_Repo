using System;
using System.Diagnostics;
using System.Linq.Expressions;

class MonteCarlo
{
    static void Main()
    {
        double squareSize, radius;
        Console.WriteLine("Input circle radius:");
        radius = double.Parse(Console.ReadLine());
        squareSize = 2*radius;
        Console.WriteLine("Input the number of points:");
        int totalPoints = int.Parse(Console.ReadLine());

        Random random = new Random();
        int pointsInside = 0;

        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < totalPoints; i++)
        {
            double x = random.NextDouble() * squareSize;
            double y = random.NextDouble() * squareSize;
            if(IsPointInside(x, y, radius))
            {
                pointsInside++;
            }
        }

        double approximateArea = (totalPoints - (double)pointsInside) / totalPoints * Math.Pow(squareSize,2); 
        sw.Stop();

        Console.WriteLine($"Approximate area: {approximateArea}");

        double exactArea = CalculateExactArea(radius);

        Console.WriteLine($"Exact area: {exactArea}");

        double error = Math.Abs((exactArea - approximateArea) / exactArea) * 100;

        Console.WriteLine($"Margin of error: {error} %");
        Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");
    }

    static bool IsPointInside(double x, double y, double radius)
    {

        return (x * x) + (y * y) <= radius * radius;
    }

    static double CalculateExactArea(double radius)
    {
        return Math.PI * radius * radius;
    }

}