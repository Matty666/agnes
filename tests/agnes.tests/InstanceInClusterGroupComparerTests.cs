using System;
using System.Collections.Generic;
using agnes.ClusterSet;
using Shouldly;
using Xunit;

namespace agnes.tests
{
    public class InstanceInClusterGroupComparerTests
    {
        private SortedClusterResult<TestClusterCandidate> GetClusterResult(params IEnumerable<int>[] sets) =>
            new SortedClusterResult<TestClusterCandidate>(TestClusterCandidate.ToClusterSets(sets), TestClusterCandidate.SortingKeyFunction);

        private static readonly IEnumerable<int>[] ClusterData = {new[] {700, 710}, new[] {20, 10}};

        private readonly InstanceInClusterGroupComparer<TestClusterCandidate> _subject;

        public InstanceInClusterGroupComparerTests()
        {
            _subject = new InstanceInClusterGroupComparer<TestClusterCandidate>(GetClusterResult(ClusterData));
        }

        [Fact]
        public void NullsAreEqual()
        {
            _subject.Compare(null, null).ShouldBe(0);
        }

        [Fact]
        public void NullXIsLessThanY()
        {
            _subject.Compare(null, new TestClusterCandidate(10)).ShouldBe(-1);
        }

        [Fact]
        public void XIsGreaterThanNullY()
        {
            _subject.Compare(new TestClusterCandidate(10), null).ShouldBe(1);
        }

        [Fact]
        public void XNotInAnyClusterThrows()
        {
            Should.Throw<Exception>(() => _subject.Compare(new TestClusterCandidate(0), new TestClusterCandidate(20)));
        }

        [Fact]
        public void YNotInAnyClusterThrows()
        {
            Should.Throw<Exception>(() => _subject.Compare(new TestClusterCandidate(10), new TestClusterCandidate(21)));
        }

        [Fact]
        public void XAndYNotInAnyClusterThrows()
        {
            Should.Throw<Exception>(() => _subject.Compare(new TestClusterCandidate(0), new TestClusterCandidate(21)));
        }

        [Fact]
        public void XAndYInSameClusterShouldBeEqual()
        {
            _subject.Compare(new TestClusterCandidate(10), new TestClusterCandidate(20)).ShouldBe(0);
        }

        [Fact]
        public void XInBiggerClusterShouldBeGreaterThanY()
        {
            _subject.Compare(new TestClusterCandidate(700), new TestClusterCandidate(20)).ShouldBe(1);
        }

        [Fact]
        public void YInBiggerClusterShouldBeGreaterThanX()
        {
            _subject.Compare(new TestClusterCandidate(10), new TestClusterCandidate(710)).ShouldBe(-1);
        }
    }
}
