using MandelbrotLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetMQ;
using NetMQ.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Mandelbrot
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        private List<Result> receivedResults;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread sink = new Thread(StartSink);
            sink.Start();
        }

        private void StartSink()
        {
            this.receivedResults = new List<Result>();

            using (var receiver = new PullSocket("@tcp://localhost:400"))
            {
                for (int taskNumber = 0; taskNumber < 400; taskNumber = taskNumber + 10)
                {
                    var workerDoneTrigger = receiver.ReceiveFrameBytes();
                    List<Result> gameField = null;
                    BinaryFormatter binaryFormatter2 = new BinaryFormatter();

                    using (var memoryStream2 = new MemoryStream(workerDoneTrigger))
                    {
                        gameField = (List<Result>)binaryFormatter2.Deserialize(memoryStream2);
                        this.receivedResults.AddRange(gameField);
                    }
                }
            }

            Bitmap bm = new Bitmap(400, 400);

            foreach (var item in this.receivedResults)
            {
                bm.SetPixel(item.X, item.Y, item.Iteration < 100 ? Color.Black : Color.White);
            }

            pictureBox1.Image = bm;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                using (var sender = new PushSocket("@tcp://localhost:8081"))
                using (var sink = new PullSocket(">tcp://localhost:8080"))
                {
                    Thread.Sleep(1000);

                    int upper = 10;
                    int height = 400;
                    for (int lower = 0; lower < height; lower += 10)
                    {

                        sender.SendFrame(lower + "," + upper + "," + height);
                        upper += 10;

                    }
                }
            });
        }
    }
}
