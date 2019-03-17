using System;
using System.Collections.Generic;
using System.Linq;

namespace agnes.ClusterSet
{
    public class InstanceInClusterGroupComparer<T> : IComparer<T>
        where T : IComparable<T>, IEquatable<T>
    {
        private readonly SortedClusterResult<T> _clusters;

        public InstanceInClusterGroupComparer(SortedClusterResult<T> clusters)
        {
            _clusters = clusters;
        }

        public int Compare(T x, T y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null)) return 0;
            if (ReferenceEquals(null, x)) return -1;
            if (ReferenceEquals(y, null)) return 1;

            var xCluster = _clusters.Single(c => c.Contains(x));
            var yCluster = _clusters.Single(c => c.Contains(y));
            return xCluster.CompareTo(yCluster);
        }
    }
}
