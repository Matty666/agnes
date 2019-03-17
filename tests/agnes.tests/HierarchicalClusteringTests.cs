using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using agnes.HCA;
using Shouldly;
using Xunit;

namespace agnes.tests
{
    public class HierarchicalClusteringTests
    {
        private readonly HierarchicalClustering<TestClusterCandidate> _subject;

        public HierarchicalClusteringTests()
        {
            _subject = new HierarchicalClustering<TestClusterCandidate>(TestClusterCandidate.CompleteLinkage);
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
        public void Clusters(List<TestClusterCandidate> candidates, double cutPoint,
            Func<Cluster<TestClusterCandidate>, Cluster<TestClusterCandidate>, double> linkageFunction,
            ISet<ISet<TestClusterCandidate>> expectedClusterResults)
        {
            var subject = new HierarchicalClustering<TestClusterCandidate>(linkageFunction);
            var results = subject.Cluster(candidates);
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
                TestClusterCandidate.ToTestList(1, 2, 3, 10, 99, 500, 1000, 5000, 9000, 9001, 9002, 9003, 9004),
                1,
                LinkageFunctions.CompleteLinkage((TestClusterCandidate i1, TestClusterCandidate i2) => Math.Abs(Math.Log10(i1.Value) - Math.Log10(i2.Value))),
                TestClusterCandidate.ToClusterSets(new[] {1, 2, 3}, new[] {10, 99}, new[] {500, 1000}, new[] {5000, 9000, 9001, 9002, 9003, 9004})
            };

            yield return new object[]
            {
                TestClusterCandidate.ToTestList(10, 20, 499, 501, 700, 710),
                200,
                TestClusterCandidate.CompleteLinkage,
                TestClusterCandidate.ToClusterSets(new[] {10, 20}, new[] {499, 501}, new[] {700, 710})
            };

            yield return new object[]
            {
                TestClusterCandidate.ToTestList(5, 500, 4, 603),
                200,
                TestClusterCandidate.CompleteLinkage,
                TestClusterCandidate.ToClusterSets(new[] {500, 603}, new[] {5, 4})
            };

            yield return new object[]
            {
                TestClusterCandidate.ToTestList(5, 500, 4, 603, 5000),
                200,
                TestClusterCandidate.CompleteLinkage,
                TestClusterCandidate.ToClusterSets(new[] {500, 603}, new[] {5, 4}, new[] {5000})
            };

            yield return new object[]
            {
                TestClusterCandidate.ToTestList(5, 500, 501, 430, 4, 603, 5000),
                200,
                TestClusterCandidate.CompleteLinkage,
                TestClusterCandidate.ToClusterSets(new[] {500, 603, 501, 430}, new[] {5, 4}, new[] {5000})
            };

            yield return new object[]
            {
                TestClusterCandidate.ToTestList(1, 2, 3, 4, 5, 6, 7),
                200,
                TestClusterCandidate.CompleteLinkage,
                TestClusterCandidate.ToClusterSets(new[] {1, 2, 3, 4, 5, 6, 7})
            };

            yield return new object[]
            {
                TestClusterCandidate.ToTestList(700, 499, 20, 501, 10, 710),
                200,
                TestClusterCandidate.SingleLinkage,
                TestClusterCandidate.ToClusterSets(new[] {10, 20}, new[] {499, 501, 700, 710})
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
