using System;

namespace OdometryApp
{
    public class Position
    {
        public Position(double x, double y, double heading)
        {
            X = x;
            Y = y;
            Heading = heading;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Heading { get; set; }
    }
}