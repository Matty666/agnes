using System;
using System.Collections.Generic;
using System.Linq;

namespace agnes
{
    public interface IAnalyseClusters<T>
    {
        Cluster<T> Cluster(IEnumerable<T> candidates);
    }

    public class HierarchicalClustering<T> : IAnalyseClusters<T>
    {
        private readonly DistanceMatrixCalculator<T> _distanceMatrixCalculator;

        public HierarchicalClustering(Func<Cluster<T>, Cluster<T>, double> linkageFunction)
        {
            _distanceMatrixCalculator = new DistanceMatrixCalculator<T>(linkageFunction);
        }

        public Cluster<T> Cluster(IEnumerable<T> candidates)
        {
            if(candidates == null) return Cluster<T>.Empty();

            var clusters = candidates.Select(c => new Cluster<T>(null, null, 0, c)).ToArray();

            if (clusters.Length <= 1) return clusters.SingleOrDefault() ?? Cluster<T>.Empty();

            do
            {
                // 1. construct distance matrix for clusters
                var distanceMatrix = _distanceMatrixCalculator.Calculate(clusters);

                // 2. find 2 clusters with the smallest distance
                var minClusterPair = FindNearestClusters(clusters, distanceMatrix);

                // 3. merge the 2 clusters, remove the two merged clusters from the list and append the merged cluster
                clusters = clusters
                    .Where(c => !minClusterPair.ImmediatelyContains(c))
                    .Append(minClusterPair)
                    .ToArray();

                // 4. repeat until all clusters are merged (one cluster left in the list, with left and right nodes)
                // alternatively the number of steps is the number of initial clusters - 1 (as the list is reduced by one each iteration)
            } while (clusters.Length > 1 || !clusters.First().IsMerged);

            return clusters.Single();
        }

        private static Cluster<T> FindNearestClusters(Cluster<T>[] clusters, double[,] distanceMatrix)
        {
            Cluster<T> minClusterPair = null;
            var minDistance = double.MaxValue;

            void DifferentIndexAction(int xIndex, int yIndex)
            {
                var distance = distanceMatrix[xIndex, yIndex];
                if (distance < minDistance)
                {
                    minClusterPair = new Cluster<T>(clusters[xIndex], clusters[yIndex], distance);
                    minDistance = distance;
                }

            }

            ReflectiveMatrix.Iterate((x, y) => { }, DifferentIndexAction, clusters.Length);

            return minClusterPair;
        }
    }
}
