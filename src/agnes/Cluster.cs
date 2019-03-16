using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace agnes
{
    public class Cluster<T> : IEnumerable<T>
    {
        private readonly Cluster<T> _left;
        private readonly Cluster<T> _right;
        private readonly T _instance;

        public readonly double Distance;

        public static Cluster<T> Empty() => new Cluster<T>();

        public IEnumerable<T> Contents
        {
            get
            {
                if (_left == null && _right == null && _instance == null) return new List<T>();
                if (_left == null && _right == null) return new List<T> {_instance};

                var leftContents = _left?.Contents ?? new List<T>();
                var rightContents = _right?.Contents ?? new List<T>();
                
                return leftContents.Concat(rightContents).ToArray();
            }
        }

        private Cluster() : this(null, null, 0, default(T))
        {

        }

        public Cluster(Cluster<T> left, Cluster<T> right, double distance)
            : this(left, right, distance, default(T))
        {
            
        }

        public Cluster(Cluster<T> left, Cluster<T> right, double distance, T instance)
        {
            _left = left;
            _right = right;
            _instance = instance;
            Distance = distance;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Contents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool IsMerged => _left != null && _right != null && _instance == null;

        public bool ImmediatelyContains(Cluster<T> cluster) => cluster == _left || cluster == _right;

        public ISet<ISet<T>> GetClusteredInstances(Func<double, bool> predicate)
        {
            if (!IsMerged) return new HashSet<ISet<T>> {new HashSet<T> {_instance}};

            var left = _left.GetClusteredInstances(predicate);
            var right = _right.GetClusteredInstances(predicate);

            if (predicate(Distance))
            {
                return new HashSet<ISet<T>>(left.Concat(right));
            }

            return new HashSet<ISet<T>>{ new HashSet<T>(left.SelectMany(s => s).Concat(right.SelectMany(s => s)))};
        }
    }
}