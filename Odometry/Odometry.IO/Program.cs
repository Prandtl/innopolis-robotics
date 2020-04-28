using System;
using System.Linq;

namespace Odometry.IO
{
    class Program
    {
        static void Main(string[] args)
        {
            var setup = Console.ReadLine().Split(' ');
            var iterations = int.Parse(setup[0]);
            var stepSize = double.Parse(setup[1]);
            var app = new OdometryApp.OdometryApp(iterations, stepSize);
            for (int i = 0; i < iterations; i++)
            {
                var input = Console.ReadLine().Split(' ').Select(double.Parse).ToArray();
                var p = app.UpdatePosition(input[0], input[1]);
                Console.WriteLine($"X: {p.X}, Y: {p.Y}, heading: {p.Heading}");
            }
        }
    }
}