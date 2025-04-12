
namespace MatrixWordFinder
{
    public abstract class WordFinder
    {
        public List<string> matrix { get; }
        public int RowCount { get; }
        public int ColumnCount { get; }


        public WordFinder(IEnumerable<string> matrix)
        {
            this.matrix = matrix.ToList();
            this.ColumnCount = matrix.First().Length; //All strings in the matrix should have the same length; each string is a row
            this.RowCount = matrix.Count();
        }

        public abstract IEnumerable<string> Find(IEnumerable<string> wordStream);
    }
}
