using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixWordFinder
{
    public static class Helpers
    {
        /* <summary>
         * Remove duplicates and words whose length is greater than the matrix's RowCount AND ColumnCount
         */
        public static IEnumerable<string> FilterWordStream(IEnumerable<string> wordStream, int rowCount, int columnCount)
        {
            return wordStream
                .Distinct()
                .Where(word => word.Length <= rowCount && word.Length <= columnCount);
        }
    }
}
