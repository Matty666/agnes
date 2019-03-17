using System;
using System.Linq;

namespace agnes.HCA
{
    public static class LinkageFunctions
    {
        public static Func<Cluster<T>, Cluster<T>, double> SingleLinkage<T>(
            Func<T, T, double> distanceFunction)
        {
            return (c1, c2) => { return c1.Min(i1 => c2.Min(i2 => distanceFunction(i1, i2))); };
        }

        public static Func<Cluster<T>, Cluster<T>, double> CompleteLinkage<T>(
            Func<T, T, double> distanceFunction)
        {
            return (c1, c2) => { return c1.Max(i1 => c2.Max(i2 => distanceFunction(i1, i2))); };
        }
    }
}