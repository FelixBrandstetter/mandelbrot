using System;
using System.Collections.Generic;
using System.Text;

namespace MandelbrotLibrary
{
    public class CalcServerInfo
    {
        public CalcServerInfo(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public CalcServerInfo()
        {

        }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
