using System;
using System.Collections.Generic;
using System.Linq;

namespace agnes
{
    internal class DistanceMatrixCalculator<T>
    {
        private readonly Func<Cluster<T>, Cluster<T>, double> _linkageFunction;

        public DistanceMatrixCalculator(Func<Cluster<T>, Cluster<T>, double> linkageFunction)
        {
            _linkageFunction = linkageFunction;
        }

        public double[,] Calculate(IEnumerable<Cluster<T>> clusters)
        {
            var c = clusters.ToArray();

            var results = new double[c.Length,c.Length];

            void DifferentIndexAction(int xIndex, int yIndex)
            {
                results[xIndex, yIndex] = _linkageFunction(c[xIndex], c[yIndex]);
                results[yIndex, xIndex] = results[xIndex, yIndex];
            }

            ReflectiveMatrix.Iterate(GetSameIndexAction(results), DifferentIndexAction, c.Length);

            return results;
        }

        private static Action<int, int> GetSameIndexAction(double[,] results)
        {
            return (x, y) => results[x, y] = 0;
        }
    }
}