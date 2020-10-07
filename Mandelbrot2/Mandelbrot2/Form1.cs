using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrot2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public List<(int, int, Color)> CalculatePixels(Bitmap bm, int lower, int upper)
        {
            var list = new List<(int, int, Color)>();

            for (int x = lower; x < upper; x++)
            {
                for (int y = 0; y < pictureBox1.Height; y++)
                {
                    double a = (double)(x - (pictureBox1.Width / 2)) / (double)(pictureBox1.Width / 4);
                    double b = (double)(y - (pictureBox1.Height / 2)) / (double)(pictureBox1.Height / 4);
                    Complex c = new Complex(a, b);
                    Complex z = new Complex(0, 0);
                    int it = 0;
                    do
                    {
                        it++;
                        z.Square();
                        z.Add(c);
                        if (z.Magnitude() > 2.0) break;
                    }
                    while (it < 100);

                    list.Add((x, y, it < 100 ? Color.Black : Color.White));
                    //bm.SetPixel(x, y, it < 100 ? Color.Black : Color.White);
                }
            }

            return list;
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            var tasks = new List<Task<List<(int, int, Color)>>>();

            for (int i = 0; i < pictureBox1.Width; i += 100)
            {
                if (i + 100 >= pictureBox1.Width)
                {
                    int s = i;

                    var finalTask = Task.Run(() =>
                    {
                        return CalculatePixels(bm, s, pictureBox1.Width);
                    });

                    tasks.Add(finalTask);

                    break;
                }

                int o = i;

                var task = Task.Run(() =>
                {
                    return this.CalculatePixels(bm, o, o + 100);
                });

                tasks.Add(task);
            }

            //var task1 = Task.Run(() => {
            //           return this.CalculatePixels(bm, 0, 100);
            //           });

            //var task2 = Task.Run(() => {
            //    return this.CalculatePixels(bm, 100, 200);
            //});

            //var task3 = Task.Run(() => {
            //    return this.CalculatePixels(bm, 100, 300);
            //});

            //tasks.Add(task1);
            //tasks.Add(task2);
            //tasks.Add(task3);

            await Task.WhenAll(tasks.ToArray());

            foreach (var task in tasks)
            {
                foreach (var result in task.Result)
                {
                    bm.SetPixel(result.Item1, result.Item2, result.Item3);
                }
            }
            

           // Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //for (int x = 0; x < pictureBox1.Width; x++)
            //{
            //    for (int y = 0; y < pictureBox1.Height; y++)
            //    {
            //        double a = (double)(x - (pictureBox1.Width / 2)) / (double)(pictureBox1.Width / 4);
            //        double b = (double)(y - (pictureBox1.Height / 2)) / (double)(pictureBox1.Height / 4);
            //        Complex c = new Complex(a, b);
            //        Complex z = new Complex(0, 0);
            //        int it = 0;
            //        do
            //        {
            //            it++;
            //            z.Square();
            //            z.Add(c);
            //            if (z.Magnitude() > 2.0) break;
            //        }
            //        while (it < 100);
            //        bm.SetPixel(x, y,it<100?Color.Black:Color.White);
            //    }
            //}


            pictureBox1.Image = bm;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
