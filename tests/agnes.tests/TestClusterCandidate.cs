using System;
using System.Collections.Generic;
using System.Linq;

namespace agnes.tests
{
    public class TestClusterCandidate : IEquatable<TestClusterCandidate>, IComparable<TestClusterCandidate>
    {
        internal static ISet<ISet<TestClusterCandidate>> ToClusterSets(params IEnumerable<int>[] values) =>
            new HashSet<ISet<TestClusterCandidate>>(values.Select(ss =>
                new HashSet<TestClusterCandidate>(ss.Select(v => new TestClusterCandidate(v)))));

        internal static IList<TestClusterCandidate> ToTestList(params int[] values) =>
            values.Select(v => new TestClusterCandidate(v)).ToList();

        public static int SortingKeyFunction(ISet<TestClusterCandidate> cluster)
        {
            return Convert.ToInt32(cluster.Average(c => c.Value));
        }

        public readonly int Value;

        public TestClusterCandidate(int value) => Value = value;

        public bool Equals(TestClusterCandidate other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public double GetDistanceBetween(TestClusterCandidate other)
        {
            return Math.Abs(Value - other.Value);
        }

        public int CompareTo(TestClusterCandidate other)
        {
            if (ReferenceEquals(null, other)) return 1;
            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestClusterCandidate) obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}