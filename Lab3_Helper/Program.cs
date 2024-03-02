using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class MonteCarloServer
{
    static void Main()
    {
        TcpListener server = null;
        try
        {
            int port = 8888;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);
            server.Start();

            Console.WriteLine("Server is waiting for a connection...");

            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected!");

            NetworkStream stream = client.GetStream();

            // Receive data from client (radius and random numbers array)
            byte[] buffer = new byte[9506238];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string clientData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            string[] dataParts = clientData.Split(';');

            double radius = double.Parse(dataParts[0]);
            string[] randomNumbersString = dataParts[1].Split(',');

            double[] randomNumbers = new double[randomNumbersString.Length];
            for (int i = 0; i < randomNumbersString.Length; i++)
            {
                randomNumbers[i] = double.Parse(randomNumbersString[i]);
            }

            int totalPoints = randomNumbers.Length / 2;

            int pointsInside = 0;
            int index = 0;

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

            // Send the result back to the client
            string result = pointsInside.ToString();
            byte[] resultBytes = Encoding.ASCII.GetBytes(result);
            stream.Write(resultBytes, 0, resultBytes.Length);
            Console.WriteLine($"Algorithm finished, points inside: {result}");

            // Clean up
            stream.Close();
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
        finally
        {
            server?.Stop();
        }
    }

    static bool IsPointInside(double x, double y, double radius)
    {
        return (x * x) + (y * y) <= radius * radius;
    }
}