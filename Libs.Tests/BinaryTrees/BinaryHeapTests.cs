using System.Linq;
using Xunit;

namespace Libs.Tests.BinaryTrees
{
    public class BinaryHeapTests
    {

        [Fact]
        public void ExtractMin_ShouldExtractMin()
        {
            var heap = new BinaryHeap<string>();
            var elements = new[] { 3, 2, 1, 4 };

            heap.BuildHeap(elements.Select(e => new HeapNode<string>(e.ToString(), e)));

            Assert.Equal("1", heap.ExtractMin().Element);
            Assert.Equal("2", heap.ExtractMin().Element);
            Assert.Equal("3", heap.ExtractMin().Element);
            Assert.Equal("4", heap.ExtractMin().Element);
        }
    }
}
