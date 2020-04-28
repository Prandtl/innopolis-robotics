using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OdometryApp;

namespace Odometry
{
    public class OdometryAppWrapper
    {
        public async Task<Position[]> RunOdometryApp(string inputsFile)
        {
            
            using (var s = new StreamReader(new FileStream(inputsFile, FileMode.Open)))
            {
                var setup = s.ReadLine().Split(' ');
                var iterations = int.Parse(setup[0]);
                var stepSize = double.Parse(setup[1]);
                var app = new OdometryApp.OdometryApp(iterations, stepSize);
                for (int i = 0; i < iterations; i++)
                {
                    var input = s.ReadLine().Replace(',','.').Split(' ').Select(x=>double.Parse(x, CultureInfo.InvariantCulture)).ToArray();
                    var p = app.UpdatePosition(input[0], input[1]);
                }

                return app.GetHistory();
            }
        }
    }
}