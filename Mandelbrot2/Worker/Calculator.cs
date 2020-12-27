using MandelbrotLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Worker
{
    public class Calculator
    {
        public static List<Result> CalculatePixels(int lower, int upper, int height, int width)
        {
            var list = new List<Result>();

            for (int x = lower; x < upper; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double a = (double)(x - (width / 2)) / (double)(width / 4);
                    double b = (double)(y - (height / 2)) / (double)(height / 4);
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

                    list.Add(new Result(x, y, it));
                }
            }

            return list;
        }

        //public async Task<List<Result>> CalculateArea()
        //{
        //    var tasks = new List<Task<List<Result>>>();

        //    for (int i = 0; i < 300; i += 100)
        //    {
        //        if (i + 100 >= 325)
        //        {
        //            int s = i;

        //            var finalTask = Task.Run(() =>
        //            {
        //                return CalculatePixels(s, 300);
        //            });

        //            tasks.Add(finalTask);

        //            break;
        //        }

        //        int o = i;

        //        var task = Task.Run(() =>
        //        {
        //            return this.CalculatePixels(o, o + 100);
        //        });

        //        tasks.Add(task);
        //    }

        //    await Task.WhenAll(tasks.ToArray());

        //    List<Result> convertedResults = new List<Result>();

        //    foreach (var task in tasks)
        //    {
        //        foreach (var result in task.Result)
        //        {
        //            convertedResults.Add(new Result(result.X, result.Y, result.Iteration));
        //        }
        //    }

        //    return convertedResults;
        //}
    }
}
