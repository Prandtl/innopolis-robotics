using System;
using System.Linq;

namespace Odometry.IO
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new OdometryApp.OdometryApp(()=>Console.ReadLine());
            app.Run();
            foreach (var p in app.GetHistory()) {
                Console.WriteLine($"X: {p.X}, Y: {p.Y}, heading: {p.Heading}");
            }
        }
    }
}