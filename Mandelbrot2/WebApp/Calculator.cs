using MandelbrotLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public class Calculator
    {
        public List<Result> CalculatePixels(int lower, int upper)
        {
            var list = new List<Result>();

            for (int x = lower; x < upper; x++)
            {
                for (int y = 0; y < 325; y++)
                {
                    double a = (double)(x - (300 / 2)) / (double)(300 / 4);
                    double b = (double)(y - (325 / 2)) / (double)(325 / 4);
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

        public async Task<List<Result>> CalculateArea()
        {
            var tasks = new List<Task<List<Result>>>();

            for (int i = 0; i < 300; i += 100)
            {
                if (i + 100 >= 325)
                {
                    int s = i;

                    var finalTask = Task.Run(() =>
                    {
                        return CalculatePixels(s, 300);
                    });

                    tasks.Add(finalTask);

                    break;
                }

                int o = i;

                var task = Task.Run(() =>
                {
                    return this.CalculatePixels(o, o + 100);
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks.ToArray());

            List<Result> convertedResults = new List<Result>();

            foreach (var task in tasks)
            {
                foreach (var result in task.Result)
                {
                    convertedResults.Add(new Result(result.X, result.Y, result.Iteration));
                }
            }

            return convertedResults;
        }
    }
}
