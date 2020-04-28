using System;

namespace OdometryApp
{
    public class OdometryApp
    {
        private Position[] history;
        private int iteration;
        private double stepInterval;


        public Position UpdatePosition(double linear, double angular)
        {
            var p =  CalculateNextPosition(linear, angular);
            history[iteration] = p;
            iteration++;
            return p;
        }

        private Position CalculateNextPosition(double linear, double angular)
        {
            var previous = iteration == 0 ? new Position(0, 0, 0) : history[iteration - 1];
            var updateLinear = linear * stepInterval;
            var updateAngular = angular * stepInterval;
            var newHeading = previous.Heading + updateAngular;
            var newX = previous.X + updateLinear * Math.Cos(newHeading);
            var newY = previous.Y + updateLinear * Math.Sin(newHeading);
            return new Position(newX, newY, newHeading);
        }

        public Position[] GetHistory()
        {
            return history;
        }

        public OdometryApp(int samplesCount, double stepInterval)
        {
            this.history = new Position[samplesCount];
            this.stepInterval = stepInterval;
            this.iteration = 0;
        }
    }
}