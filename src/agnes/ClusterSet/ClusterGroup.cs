using System;
using System.Collections;
using System.Collections.Generic;

namespace agnes.ClusterSet
{
    public class ClusterGroup<T> : IComparable<ClusterGroup<T>>, IEnumerable<T>
    {
        private readonly ISet<T> _instances;
        private readonly int _sortingKey;

        public ClusterGroup(ISet<T> instances, int sortingKey)
        {
            _instances = instances;
            _sortingKey = sortingKey;
        }

        public int CompareTo(ClusterGroup<T> other)
        {
            if (other == null) return 1;
            return _sortingKey.CompareTo(other._sortingKey);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _instances.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}