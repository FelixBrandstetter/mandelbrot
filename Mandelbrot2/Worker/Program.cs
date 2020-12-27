using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NetMQ;
using NetMQ.Sockets;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var receiver = new PullSocket(">tcp://localhost:8081"))
            using (var sender = new PushSocket(">tcp://localhost:400"))
            {
                Console.WriteLine("Waiting for Ventilator...");

                while (true)
                {
                    string workload = receiver.ReceiveFrameString();
                    Console.WriteLine("RECEIVED");

                    string[] workLoadConv = workload.Split(',');
                    int lower = int.Parse(workLoadConv[0]);
                    int upper = int.Parse(workLoadConv[1]);
                    int width = int.Parse(workLoadConv[2]);
                    int height = 400;

                    var calculatedResult = Calculator.CalculatePixels(lower, upper, height, width);

                    byte[] convertedBytes;
                    BinaryFormatter formatter = new BinaryFormatter();

                    using (var memoryStream = new MemoryStream())
                    {
                        formatter.Serialize(memoryStream, calculatedResult);
                        convertedBytes = memoryStream.ToArray();
                    }

                    Console.WriteLine("Successfully sent to sink");
                    sender.SendFrame(convertedBytes);
                }
            }
        }
    }
}
