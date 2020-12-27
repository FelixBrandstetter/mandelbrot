using NetMQ;
using NetMQ.Sockets;
using System;

namespace Ventilator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====== VENTILATOR ======");
            using (var sender = new PushSocket("@tcp://localhost:8081"))
            using (var sink = new PullSocket(">tcp://localhost:400"))
            {
                Console.WriteLine("PRESS ENTER WHEN READY");
                Console.ReadKey();

                int upper = 10;
                int height = 400;

                for (int lower = 0; lower < 400; lower += 10)
                {
                    sender.SendFrame(lower + "," + upper + "," + height);
                    upper += 10;
                }
            }

            Console.WriteLine("FINISHED");
            Console.ReadKey();
        }
    }
}
