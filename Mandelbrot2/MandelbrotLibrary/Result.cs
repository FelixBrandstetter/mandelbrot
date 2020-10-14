using System;

namespace MandelbrotLibrary
{
    public class Result
    {
        public Result(int x, int y, int iteration)
        {
            this.X = x;
            this.Y = y;
            this.Iteration = iteration;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Iteration { get; set; }
    }
}
