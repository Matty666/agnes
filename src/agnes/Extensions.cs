using System;
using System.Collections.Generic;
using System.Text;
using agnes.Enumerable;

namespace agnes
{
    public static class Extensions
    {
        public static readonly Func<double, double, double> CompleteLinkage = Math.Max;
        public static readonly Func<double, double, double> SingleLinkage = Math.Min;

        public static IClusteredEnumerable<TElement> ClusterBy<TElement>(this IEnumerable<TElement> source, Func<TElement, double> distanceValueSelector, double clusterDistanceCutPoint)
        {
            double DistanceFunction(TElement left, TElement right) => Math.Abs(distanceValueSelector(left) - distanceValueSelector(right));

            return new ClusteredEnumerable<TElement>(source, DistanceFunction, CompleteLinkage, clusterDistanceCutPoint, distanceValueSelector);
        }
    }
}
