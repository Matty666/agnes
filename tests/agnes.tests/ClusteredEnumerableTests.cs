using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace agnes.tests
{

    public class ClusteredEnumerableTests
    {
        [Fact]
        public void Cluster()
        {
            var source = new List<int> { 101, 204, 203, 105, 103 };
            var results = source.ClusterBy(v => v % 100, 2).ToList();
            results.Count.ShouldBe(2);
            results.First().ShouldBe(new List<int> {204, 105});
            results.Last().ShouldBe(new List<int> {101, 203, 103});
        }
    }
}
