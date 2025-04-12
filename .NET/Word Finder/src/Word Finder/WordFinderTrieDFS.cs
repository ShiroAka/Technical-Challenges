
namespace MatrixWordFinder
{    
    public class WordFinderTrieDFS : WordFinder
    {
        public WordFinderTrieDFS(IEnumerable<string> matrix) : base(matrix) { }

        public override IEnumerable<string> Find(IEnumerable<string> wordStream)
        {
            IEnumerable<string> filteredWordStream = Helpers.FilterWordStream(wordStream, RowCount, ColumnCount);
            Trie trie = new Trie();
            
            foreach (string word in filteredWordStream)
            {
                trie.Insert(word);
            }
                        
            Dictionary<string, int> foundWordsDict = new Dictionary<string, int>();
            bool[,] visited = new bool[RowCount, ColumnCount]; //2D boolean array to keep track of visited cells

            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColumnCount; col++)
                {
                    DepthFirstSearch(matrix, row, col, trie.GetRoot(), foundWordsDict, visited);
                }
            }

            return foundWordsDict
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .ToList();
        }

        private void DepthFirstSearch(List<string> matrix, int row, int col, TrieNode node, Dictionary<string, int> foundWordsDict, bool[,] visited)
        {
            //All the children of the Trie's root node are the first letters of each word in the word stream
            //Then, for any node down the tree, the children is the next letter of a word (would be like doing word[i + 1])

            if (row < 0 || row >= RowCount || col < 0 || col >= ColumnCount || visited[row, col] || !node.Children.ContainsKey(matrix[row][col]))
            {
                return;
            }

            char currentChar = matrix[row][col];
            TrieNode nextNode = node.Children[currentChar];

            //If the node contains a word, it means that the word has been found
            if (nextNode.Word != null)
            {
                if (foundWordsDict.ContainsKey(nextNode.Word))
                {
                    foundWordsDict[nextNode.Word]++;
                }
                else
                {
                    foundWordsDict[nextNode.Word] = 1;
                }
            }

            visited[row, col] = true; //Mark as visited

            int[] rowOffsets = { -1, 1, 0, 0 }; //Up, Down
            int[] colOffsets = { 0, 0, -1, 1 }; //Left, Right

            for (int i = 0; i < 4; i++)
            {
                DepthFirstSearch(matrix, row + rowOffsets[i], col + colOffsets[i], nextNode, foundWordsDict, visited);
            }

            visited[row, col] = false; //Unmark
        }

        private class TrieNode
        {
            public Dictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();
            public string? Word { get; set; }
        }

        private class Trie
        {
            private readonly TrieNode root = new TrieNode();

            public void Insert(string word)
            {
                TrieNode node = root;
                foreach (char c in word)
                {
                    if (!node.Children.ContainsKey(c))
                    {
                        node.Children[c] = new TrieNode();
                    }
                    node = node.Children[c];
                }
                node.Word = word;
            }

            public TrieNode GetRoot() => root;
        }
    }
}
