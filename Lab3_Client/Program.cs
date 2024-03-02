using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

class MonteCarloClient
{
    static void Main()
    {
        try
        {
            TcpClient client = new TcpClient("127.0.0.1", 8888);
            NetworkStream stream = client.GetStream();

            Console.WriteLine("Input circle radius:");
            double radius = double.Parse(Console.ReadLine());

            Console.WriteLine("Input the number of points:");
            int totalPoints = int.Parse(Console.ReadLine());

            double squareSize = 2 * radius;
            double[] randomNumbers = new double[totalPoints * 2];
            Random random = new Random(12345);

            for (int i = 0; i < totalPoints * 2; i++)
            {
                randomNumbers[i] = random.NextDouble() * squareSize;
            }

            // Send radius and random numbers array to the server
            Stopwatch sw = Stopwatch.StartNew();
            double[] randomNumbersServer = new double[totalPoints/2];
            Array.ConstrainedCopy(randomNumbers, totalPoints/2, randomNumbersServer,0, totalPoints/2);

            string data = $"{radius};{string.Join(",", randomNumbersServer)}";
            byte[] dataBytes = Encoding.ASCII.GetBytes(data);
            stream.Write(dataBytes, 0, dataBytes.Length);

            int pointsInsideClient = 0;
            int index = 0;

            for (int i = 0; i < totalPoints/2; i++)
            {
                double x = randomNumbers[index];
                double y = randomNumbers[index + 1];
                if (IsPointInside(x, y, radius))
                {
                    pointsInsideClient++;
                }
                index += 2;
            }


            // Receive result from the server
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string serverData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            int serverPointsInside = int.Parse(serverData);
            int pointsInside = serverPointsInside + pointsInsideClient;
            double approximateArea = (totalPoints - (double)pointsInside) / totalPoints * Math.Pow(squareSize, 2);
            sw.Stop();
            Console.WriteLine($"Approximate area: {approximateArea}");
            double exactArea = CalculateExactArea(radius);
            Console.WriteLine($"Exact area: {exactArea}");
            double error = Math.Abs((exactArea - approximateArea) / exactArea) * 100;
            Console.WriteLine($"Margin of error: {error} %");
            Console.WriteLine($"Elapsed time: {sw.Elapsed}");
            sw.Stop();
            stream.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
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