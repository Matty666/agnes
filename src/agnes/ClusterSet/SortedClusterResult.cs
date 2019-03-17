﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace agnes.ClusterSet
{
    public class SortedClusterResult<T> : IEnumerable<ClusterGroup<T>>
    {
        private readonly ISet<ClusterGroup<T>> _clusterGroups;

        public SortedClusterResult(IEnumerable<ISet<T>> clusteredInstances, Func<ISet<T>, int> sortingKeyFunction)
        {
            _clusterGroups = new SortedSet<ClusterGroup<T>>(clusteredInstances.Select(c => new ClusterGroup<T>(c, sortingKeyFunction(c))));
        }

        public IEnumerator<ClusterGroup<T>> GetEnumerator()
        {
            return _clusterGroups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}