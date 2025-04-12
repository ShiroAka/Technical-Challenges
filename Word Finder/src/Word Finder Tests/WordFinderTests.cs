
using System.Diagnostics;

namespace MatrixWordFinder.Tests
{
    public class WordFinderTests
    {
        [Fact]
        public void WordFinderTrieDFS_Find_ShouldReturnCorrectWords()
        {
            var matrix = new List<string>
            {
                "abcd",
                "efgh",
                "ijkl",
                "mnop"
            };
            var wordStream = new List<string> { "abc", "efg", "ijk", "mnop", "ghij", "aei", "dhl", "abc", "abx" };
            var wordFinder = new WordFinderTrieDFS(matrix);

            var result = wordFinder.Find(wordStream);

            var expected = new List<string> { "abc", "efg", "ijk", "mnop", "aei", "dhl" };

            //I have to do it this way because the DFS algorithm returns the words in a random order, not
            //the order they were found in the wordStream
            Assert.All(result, x => expected.Contains(x));
        }

        [Fact]
        public void WordFinderDFS_Find_TestSpeedWithBigMatrixAndArray()
        {
            var matrix = Generate64x64Matrix();
            matrix.Add("aaaaaaaaaabbbbbbbbbbccccccccccddddddddddeeeeeeeeeeffffffffffgggg"); //64 chars long

            var wordStream = GenerateWordStream();
            var wordFinder = new WordFinderSecquentialSearch(matrix);

            var watch = Stopwatch.StartNew();
            var result = wordFinder.Find(wordStream);
            watch.Stop();

            var elapsedTime = watch.ElapsedMilliseconds;
            Console.WriteLine($"Elapsed time: {elapsedTime} ms");

            var expected = new List<string> { "aaaaaaaaaabbbbbbbbbbccccccccccddddddddddeeeeeeeeeeffffffffffgggg" };

            Assert.All(result, x => expected.Contains(x));
        }

        [Fact]
        public void WordFinderSecquentialSearch_Find_ShouldReturnCorrectWords()
        {
            var matrix = new List<string>
            {
                "abcd",
                "efgh",
                "ijkl",
                "mnop"
            };
            var wordStream = new List<string> { "abc", "efg", "ijk", "mnop", "aei", "dhl", "abc", "abx" };
            var wordFinder = new WordFinderSecquentialSearch(matrix);

            var result = wordFinder.Find(wordStream);

            var expected = new List<string> { "abc", "efg", "ijk", "mnop", "aei", "dhl" };

            //This algorithm returns the words in the order they were found in the wordStream
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WordFinderSecquentialSearch_Find_TestSpeedWithBigMatrixAndArray()
        {            
            var matrix = Generate64x64Matrix();
            matrix.Add("aaaaaaaaaabbbbbbbbbbccccccccccddddddddddeeeeeeeeeeffffffffffgggg"); //64 chars long

            var wordStream = GenerateWordStream();
            var wordFinder = new WordFinderSecquentialSearch(matrix);
            
            var result = wordFinder.Find(wordStream);

            var expected = new List<string> { "aaaaaaaaaabbbbbbbbbbccccccccccddddddddddeeeeeeeeeeffffffffffgggg" };            
            Assert.Equal(expected, result);
        }

        private List<string> Generate64x64Matrix()
        {
            var matrix = new List<string>();
            for (int i = 0; i < 64; i++)
            {
                matrix.Add(new string('a', 64));
            }
            return matrix;
        }

        private List<string> GenerateWordStream()
        {
            return new List<string>
            {
                "aaaaaaaaaabbbbbbbbbbccccccccccddddddddddeeeeeeeeeeffffffffffgggg",
                "asdaf", "fghfghj", "rtyrty", "ghjghj", "gdfgdf", "cvnvbnvbn",
                "hfghd", "gkghjkgyh", "tytyhjghjn", "rteytevsdfg", "fghfghfth", "hjgnvn",
                "ghfjfj", "ghjkghk", "vxcvdf", "mbnkgh", "khyukyu", "dgsdv",
                "iupuio", "vbmbnm", "h,.hj.hj", "hjklohl", "mmvbn", "qeqr",
                "tyuiyuo", "gjktyuj", "gdfgh", "fgjhrtyyh", "wrwgs", "dhjjfghj"                
            };
        }
    }
}
