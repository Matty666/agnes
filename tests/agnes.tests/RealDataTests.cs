using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using agnes.HCA;
using Humanizer;
using Newtonsoft.Json;
using Xunit;

namespace agnes.tests
{
    
    public class RealDataTests
    {
        private List<SizeInfo> GetTestData(string filePath)
        {
            var path = Path.IsPathRooted(filePath)
                ? filePath
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);

            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<SizeInfo>>(json);
        }

        [Fact]
        public void Test()
        {
            var releaseInfos = GetTestData(@"TestData\standard.json");
            var clusterAnalyser = new HierarchicalClustering<SizeInfo>((x, y) => Math.Abs(x.Size - y.Size),
                Math.Max);

            var clusters = clusterAnalyser.Cluster(releaseInfos);
            var instances = clusters.GetClusteredInstances(d => d > 200 * 1024 * 1024).OrderByDescending(c => c.Count).ToArray();
        }
    }

    public class SizeInfo
    {
        public long Size;
        public string Id;
        public string Title;

        public override string ToString()
        {
            return $"{Id} - {Size.Bytes().LargestWholeNumberValue:#.##} {Size.Bytes().LargestWholeNumberSymbol} - {Size}";
        }
    }
}
