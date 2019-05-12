﻿using System.Collections.Generic;

namespace agnes.ClusterSet
{
    public sealed class ClusterGroupComparer<T> : IComparer<ClusterGroup<T>>
    {
        public static readonly ClusterGroupComparer<T> Instance = new ClusterGroupComparer<T>();

        public int Compare(ClusterGroup<T> x, ClusterGroup<T> y)
        {
            if (ReferenceEquals(null, x) && ReferenceEquals(null, y)) return 0;
            if (ReferenceEquals(null, x)) return -1;

            return x.CompareTo(y);
        }
    }
}
