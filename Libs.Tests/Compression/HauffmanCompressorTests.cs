using Libs.Compression;
using System.Linq;
using Xunit;

namespace Libs.Tests.Compression
{
    public class HauffmanCompressorTests
    {
        [Theory]
        [InlineData(new[] { "aaabc"},'a')]
        [InlineData(new[] { "aaaa", "bcdefg" },'a')]
        public void MoreFrequentCharacter_ShouldHavePathWithLengthOne(string[] texts, char moreFreqChar)
        {
            var compressor = new HauffmanCompressor();
            compressor.Fit(texts);

            Assert.Single(compressor.GetCharPath(moreFreqChar));
        }

        [Theory]
        [InlineData("a")]
        [InlineData("11")]
        [InlineData("aaabc")]
        [InlineData("Hello World")]
        [InlineData(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit, 
        sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.")]
        public void EncodeDecode_ShouldGiveBacktheSameString(string txt)
        {
            var compressor = new HauffmanCompressor();
            var encoded = compressor.FitEncode(new[] { txt });

            Assert.Equal(txt, compressor.Decode(encoded).First());
        }
    }
}
