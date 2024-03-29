﻿using System;
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
        double[] randomNumbers = new double[totalPoints*2];   
        Random random = new Random(12345);
        for(int i = 0; i<totalPoints*2;i++)
        {
            randomNumbers[i] = random.NextDouble() * squareSize;
        }
        int pointsInside = 0;
        int index = 0;
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < totalPoints; i++)
        {
            double x = randomNumbers[index];
            double y = randomNumbers[index + 1];
            if (IsPointInside(x, y, radius))
            {
                pointsInside++;
            }
            index += 2;
        }

        double approximateArea = (totalPoints - (double)pointsInside) / totalPoints * Math.Pow(squareSize,2); 
        sw.Stop();

        Console.WriteLine($"Approximate area: {approximateArea}");

        double exactArea = CalculateExactArea(radius);

        Console.WriteLine($"Exact area: {exactArea}");

        double error = Math.Abs((exactArea - approximateArea) / exactArea) * 100;

        Console.WriteLine($"Margin of error: {error} %");
        Console.WriteLine($"Elapsed time: {sw.Elapsed}");
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