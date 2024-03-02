using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the radius of circle:");
        int radius = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter the number of threads:");
        int numThreads = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the number of points:");
        int PointsNum = int.Parse(Console.ReadLine());
        int iterationsPerThread = PointsNum / numThreads;
        int remainder = PointsNum % numThreads;
        int[] results = new int[numThreads];

        double[] randomNumbers = new double[PointsNum * 2];
        Random random = new Random(12345);
        for (int i = 0; i < PointsNum * 2; i++)
        {
            randomNumbers[i] = random.NextDouble() * radius*2;
        }


        Stopwatch sw = Stopwatch.StartNew();
        Thread[] threads = new Thread[numThreads];
        int index = 0;
        for (int i = 0; i < numThreads; i++)
        {
            int threadIndex = i;
            threads[i] = new Thread(() =>
            {
                if (i == numThreads - 1)
                {
                    results[threadIndex] = MonteCarloSimulation(randomNumbers, index, iterationsPerThread + remainder, radius);
                }
                else
                {
                    results[threadIndex] = MonteCarloSimulation(randomNumbers, index, iterationsPerThread, radius);
                }
            });
            index += iterationsPerThread;
        }
        
        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        double totalInsideCircle = 0;
        foreach (int result in results)
        {
            totalInsideCircle += result;
        }
        sw.Stop();

        double estimatedArea = (PointsNum - totalInsideCircle) / PointsNum * Math.Pow(radius*2,2);

        Console.WriteLine($"Estimated area of the circle: {estimatedArea}");
        double exactArea = CalculateExactArea(radius);

        Console.WriteLine($"Exact area: {exactArea}");
        double error = Math.Abs((exactArea - estimatedArea) / exactArea) * 100;

        Console.WriteLine($"Margin of error: {error} %");
        Console.WriteLine($"Elapsed time: {sw.Elapsed}");
    }

    static int MonteCarloSimulation(double[] rand, int index, int iterations, double radius)
    {
        int insideCircle = 0;

        for (int i = 0; i < iterations; i++)
        {
            double x = rand[index];
            double y = rand[index+1];

            // Check if the point (x, y) is inside the unit circle
            if (x * x + y * y <= radius * radius)
            {
                insideCircle++;
            }
            index+=2;
        }

        return insideCircle;
    }
    static double CalculateExactArea(double radius)
    {
        return Math.PI * radius * radius;
    }
}