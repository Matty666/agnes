using System.Collections;
using System.Collections.Generic;
using System.Linq;
using agnes.ClusterSet;
using Shouldly;
using Xunit;

namespace agnes.tests
{
    public class SortedClusterResultTests
    {
        [Theory]
        [ClassData(typeof(SortedClusterResultTestData))]
        public void ClusterTests(ISet<ISet<TestClusterCandidate>> clusteredInstances, IList<TestClusterCandidate> expectedResults)
        {
            var subject = new SortedClusterResult<TestClusterCandidate>(clusteredInstances, TestClusterCandidate.SortingKeyFunction);
            subject.SelectMany(c => c.OrderByDescending(t => t)).ShouldBe(expectedResults);
        }
    }

    public sealed class SortedClusterResultTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                TestClusterCandidate.ToClusterSets(new[] {500, 603}, new[] {4, 5}),
                TestClusterCandidate.ToTestList(5, 4, 603, 500)
            };

            yield return new object[]
            {
                TestClusterCandidate.ToClusterSets(new[] {499, 501}, new[] {20, 10}, new[] {700, 710}),
                TestClusterCandidate.ToTestList(20, 10, 501, 499, 710, 700)
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
