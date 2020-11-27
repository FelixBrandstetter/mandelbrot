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

namespace Mandelbrot
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            var jsonString = JsonConvert.SerializeObject(new CalcServerInfo(300, 325));
            var data = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:44392/api/mandelbrot", data);
            var responseString = await response.Content.ReadAsStringAsync();
            var convertedResult = JsonConvert.DeserializeObject<List<Result>>(responseString);

            foreach (var value in convertedResult)
            {
                bm.SetPixel(value.X, value.Y, value.Iteration < 100 ? Color.Black : Color.White);
            }

            pictureBox1.Image = bm;
        }
    }
}
