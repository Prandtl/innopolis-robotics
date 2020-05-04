using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace OdometryApp
{
    public class OdometryApp
    {
        private Position[] history;
        private Func<string> getInput;
        private int iterations;
        private int iteration;
        private double iterationLength;
        private int subIterationsCount = 1000;

        public double Run()
        {
            for (int i = 0; i < this.iterations; i++) {
                var input = this.getInput().Replace(',', '.').Split(' ').Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToArray();
                var p = UpdatePosition(input[0], input[1]);
            }
            return GetIntersectionDistance();
        }

        public Position UpdatePosition(double linear, double angular)
        {
            var previous = iteration == 0 ? new Position(0, 0, 0) : history[iteration - 1];
            var updateLinear = linear * iterationLength / subIterationsCount;
            var updateAngular = angular * iterationLength / subIterationsCount;
            var intermidiatePosition = previous;
            for(int i=0; i < subIterationsCount; i++) {
                var newX = intermidiatePosition.X + updateLinear * Math.Cos(intermidiatePosition.Heading);
                var newY = intermidiatePosition.Y + updateLinear * Math.Sin(intermidiatePosition.Heading);
                var newHeading = intermidiatePosition.Heading + updateAngular;
                intermidiatePosition = new Position(newX, newY, newHeading);
            }

            var result = history[iteration] = intermidiatePosition;

            iteration++;
            return result;
        }

        public double GetIntersectionDistance()
        {
            var counter = 0;
            for (int i = 3; i < history.Length; i++)
            {
                var a1 = history[i];
                var a2 = history[i-1];
                for (int j = 1; j < i - 1; j++)
                {
                    var b1 = history[j];
                    var b2 = history[j - 1];
                    var d1 = Determinant(a1, a2, b1);
                    var d2 = Determinant(a1, a2, b2);
                    var d3 = Determinant(b1, b2, a1);
                    var d4 = Determinant(b1, b2, a2);
                    if (d1 * d2 < 0 && d3 * d4 < 0) {
                        var multiplier =
                            ((b1.X - a1.X) * (b2.Y - b1.Y) - (b1.Y - a1.Y) * (b2.X - b1.X)) /
                            ((a2.X - a1.X) * (b2.Y - b1.Y) - (a2.Y - a1.Y) * (b2.X - b1.X));
                        var crossingX = a1.X + (a2.X - a1.X) * multiplier;
                        var crossingY = a1.Y + (a2.Y - a1.Y) * multiplier;
                        Debug.WriteLine($"Iteration: {i}, {j}; Possible intersection: Line[{{{{{a1.X}, {a1.Y}}},{{{a2.X}, {a2.Y}}}}}],Line[{{{{{b1.X}, {b1.Y}}}, {{{b2.X}, {b2.Y}}}}}]");
                        return Math.Sqrt(crossingX * crossingX + crossingY * crossingY);
                    }
                }
            }
            return 0;
        }

        public double Determinant(Position origin, Position a, Position b)
        {
            return (b.X - origin.X) * (a.Y - origin.Y) - (b.Y - origin.Y) * (a.X - origin.X);
        }

        public Position[] GetHistory()
        {
            return history;
        }

        public OdometryApp(Func<string> getInput)
        {
            var setup = getInput().Replace(',', '.').Split(' ');
            this.iterationLength = double.Parse(setup[1], CultureInfo.InvariantCulture);
            this.iterations = int.Parse(setup[0], CultureInfo.InvariantCulture);
            this.history = new Position[iterations];
            this.getInput = getInput;
            this.iteration = 0;
        }
    }
}