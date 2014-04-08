using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Triggers;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var timer = new LowResourceSchedule();
            timer.Every(1.Seconds()).Action(() =>
            {
                Console.WriteLine("{0:T}", DateTime.Now);
            });

            Console.ReadKey();
        }
    }
}
