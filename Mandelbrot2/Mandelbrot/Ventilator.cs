using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Mandelbrot
{
    public class Ventilator
    {
        public void Send()
        {
            //Console.WriteLine("====== VENTILATOR ======");
            using (var sender = new PushSocket("@tcp://localhost:80"))
            using (var sink = new PullSocket(">tcp://localhost:400"))
            {
                int upper = 10;
                int height = 400;

                for (int lower = 0; lower < 400; lower += 10)
                {
                    sender.SendFrame(lower + "," + upper + "," + height);
                    upper += 10;
                }
            }
        }
    }
}
