
namespace MatrixWordFinder
{
    public class WordFinderSecquentialSearch : WordFinder
    {
        public WordFinderSecquentialSearch(IEnumerable<string> matrix) : base(matrix) { }

        public override IEnumerable<string> Find(IEnumerable<string> wordStream)
        {
            Dictionary<string, int> foundWordsDict = new Dictionary<string, int>();
            IEnumerable<string> filteredWordStream = Helpers.FilterWordStream(wordStream, RowCount, ColumnCount);

            foreach (string word in filteredWordStream)
            {
                if (FindWordInMatrix(word, out int wordFrequency))
                {
                    if (foundWordsDict.ContainsKey(word))
                    {
                        foundWordsDict[word] += wordFrequency;
                    }
                    else
                    {
                        foundWordsDict.Add(word, wordFrequency);
                    }
                }
            }
                        
            List<string> foundWords = foundWordsDict
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .ToList();

            return foundWords;
        }

        //O(2*M*N*W) --> M = nº of rows, N = nº of columns, W = nº of words in the wordStream
        //Worst case --> O(2*64*64*W) --> The challenge says the matrix is 64x64 at most
        private bool FindWordInMatrix(string word, out int wordFrequency)
        {
            wordFrequency = 0;
            bool found = false;
            string reversedWord = new string(word.Reverse().ToArray());

            //Search in columns
            for (int col = 0; col < ColumnCount; col++)
            {
                if (word.Length > RowCount)
                {
                    break; //The word cannot be longer than the length of a column
                }

                if (SearchInDirection(word, reversedWord, col, true, out int colFrequency))
                {
                    found = true;
                    wordFrequency += colFrequency;
                }
            }

            //Search in rows
            for (int row = 0; row < RowCount; row++)
            {
                if (word.Length > ColumnCount)
                {
                    break; //The word cannot be longer than the length of a row
                }

                if (SearchInDirection(word, reversedWord, row, false, out int rowFrequency))
                {
                    found = true;
                    wordFrequency += rowFrequency;
                }
            }

            return found;
        }

        private bool SearchInDirection(string word, string reversedWord, int fixedIndex, bool isColumnSearch, out int frequency)
        {
            frequency = 0;
            bool found = false;
            int charCount = isColumnSearch ? RowCount : ColumnCount;
            int wIndex = 0, rwIndex = 0;
            bool foundFirstLetter = false; //Check if the first letter of the word (or its reverse) was found in the row/column 

            for (int i = 0; i < charCount; i++)
            {
                if (wIndex == word.Length || rwIndex == word.Length)
                {
                    break; //The word has already been found
                }
                               
                char currentMatrixChar = isColumnSearch ? matrix[i][fixedIndex] : matrix[fixedIndex][i];

                if (word[wIndex] != currentMatrixChar && reversedWord[rwIndex] != currentMatrixChar)
                {
                    if (foundFirstLetter)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (word[wIndex] == currentMatrixChar)
                {
                    wIndex++;
                    foundFirstLetter = true;
                }
                else if (reversedWord[rwIndex] == currentMatrixChar)
                {
                    rwIndex++;
                    foundFirstLetter = true;
                }
            }

            //If one of the indexes reached the end of the word (word.Length - 1), and was incremented
            //before exiting the above loop, it means that the word was found
            if (wIndex == word.Length || rwIndex == word.Length)
            {
                found = true;
                frequency++;
            }

            return found;
        }
    }
}
