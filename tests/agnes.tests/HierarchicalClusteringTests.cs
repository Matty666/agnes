using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace agnes.tests
{
    public class HierarchicalClusteringTests
    {
        private readonly HierarchicalClustering<TestClusterCandidate> _subject;

        public HierarchicalClusteringTests()
        {
            var linkage = LinkageFunctions.CompleteLinkage<TestClusterCandidate>((i1, i2) => i1.GetDistanceBetween(i2));

            _subject = new HierarchicalClustering<TestClusterCandidate>(linkage);
        }

        [Fact]
        public void NullListShouldReturnEmptyCluster()
        {
            _subject.Cluster((IEnumerable<TestClusterCandidate>)null).Contents.ShouldBeEmpty();
        }

        [Fact]
        public void EmptyListShouldReturnEmptyCluster()
        {
            _subject.Cluster(new List<TestClusterCandidate>()).Contents.ShouldBeEmpty();
        }

        [Fact]
        public void SingleItemListShouldReturnSingleCluster()
        {
            var candidates = new List<TestClusterCandidate> {new TestClusterCandidate(1)};
            var results = _subject.Cluster(candidates);
            results.Contents.Single().ShouldBe(candidates.Single());
        }

        [Fact]
        public void TwoItemListShouldClusterBothItems()
        {
            var candidates = new List<TestClusterCandidate> {new TestClusterCandidate(1), new TestClusterCandidate(2)};
            var results = _subject.Cluster(candidates);
            results.Contents.ShouldBe(candidates);
        }

        [Theory]
        [ClassData(typeof(ClusteringTestData))]
        public void Clusters(List<TestClusterCandidate> candidates, double cutPoint, HashSet<HashSet<TestClusterCandidate>> expectedClusterResults)
        {
            var results = _subject.Cluster(candidates);
            var clustered = results.GetClusteredInstances(d => d > cutPoint);
            clustered.Count.ShouldBe(expectedClusterResults.Count);

            var count = expectedClusterResults
                .Sum(expectedClusterResult => clustered.Count(cluster => cluster.SetEquals(expectedClusterResult)));

            count.ShouldBe(expectedClusterResults.Count);
        }


    }

    public class ClusteringTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                ToTestList(10, 20, 499, 501, 700, 710),
                200,
                ToResultsSet(new[] {10, 20}, new[] {499, 501}, new[] {700, 710})
            };

            yield return new object[]
            {
                ToTestList(5, 500, 4, 603),
                200,
                ToResultsSet(new[] {500, 603}, new[] {5, 4})
            };

            yield return new object[]
            {
                ToTestList(5, 500, 4, 603, 5000),
                200,
                ToResultsSet(new[] {500, 603}, new[] {5, 4}, new[] {5000})
            };


            yield return new object[]
            {
                ToTestList(5, 500, 501, 430, 4, 603, 5000),
                200,
                ToResultsSet(new[] {500, 603, 501, 430}, new[] {5, 4}, new[] {5000})
            };

            yield return new object[]
            {
                ToTestList(1, 2, 3, 4, 5, 6, 7),
                200,
                ToResultsSet(new[] {1, 2, 3, 4, 5, 6, 7})
            };

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static List<TestClusterCandidate> ToTestList(params int[] values) =>
            values.Select(v => new TestClusterCandidate(v)).ToList();

        private static HashSet<HashSet<TestClusterCandidate>> ToResultsSet(params IEnumerable<int>[] values) =>
            new HashSet<HashSet<TestClusterCandidate>>(values.Select(vs => new HashSet<TestClusterCandidate>(vs.Select(v => new TestClusterCandidate(v)))));
    }

    public class TestClusterCandidate : IEquatable<TestClusterCandidate>
    {
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
