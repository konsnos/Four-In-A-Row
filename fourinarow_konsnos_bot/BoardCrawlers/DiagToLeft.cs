using FourInARow.Strategies;

namespace FourInARow.BoardCrawlers
{
    /// <example>
    /// Rows: 6
    /// Columns: 7
    /// 26	33	39	44	48	51	53	
    /// 19	25	32	38	43	47	50	52
    /// 13	18	24	31	37	42	46	49
    /// 8	12	17	23	30	36	41	45
    /// 4	7	11	16	22	29	35	40
    /// 1	3	6	10	15	21	28	34
    ///     0	2	5	9	14	20	27
    /// </example>
    class DiagToLeft : IBoardCrawler
    {
        public int[] boardLine { get; private set; }
        /// <summary>
        /// Initial row to start assigning indexes.
        /// </summary>
        private int initCol = 3;
        private int lastR;
        /// <summary>
        /// The line length of each diagonal. Each line should always reach this length in order to easily calculate the position of an index.
        /// </summary>
        private int lineLength;

        public DiagToLeft(int rowsLength, int colsLength)
        {
            lastR = rowsLength - 3;
            lineLength = rowsLength + 1;
            boardLine = new int[((colsLength - initCol) + (rowsLength - lastR) - 1) * lineLength];
        }

        public void CreateBoard(Board board)
        {
            int count = 0;
            for(int c = initCol;c<board.ColsLength;c++)
            {
                int curLineLength = 0;
                for (int r = board.RowsLength - 1;r>-1;r--)
                {
                    int c_b = c - (board.RowsLength - r - 1);
                    if (c_b > -1)
                    {
                        boardLine[count++] = board.BoardArray[r][c_b];
                        curLineLength++;
                    }
                    else
                        break;
                }

                for (; curLineLength < lineLength; curLineLength++)
                    boardLine[count++] = PatternSearchStrategy.PATTERN_BREAKER;  // Fill up line length
            }

            int maxC = board.ColsLength - 1;
            for(int r = board.RowsLength - 2;r>=lastR;r--)
            {
                int curLineLength = 0;
                for(int r_b = r; r_b > -1;r_b--)
                {
                    int c = maxC - (r-r_b);
                    if (c > -1)
                    {
                        boardLine[count++] = board.BoardArray[r_b][c];
                        curLineLength++;
                    }
                    else
                        break;
                }

                for (; curLineLength < lineLength; curLineLength++)
                    boardLine[count++] = PatternSearchStrategy.PATTERN_BREAKER;  // Fill up line length
            }
        }

        public int[] GetPos(int index, int rowsLength, int colsLength)
        {
            int diagonalCount = index / lineLength;
            int diagonalRest = index % lineLength;
            int column = initCol + diagonalCount;
            int realColumn = (column < colsLength) ? column-diagonalRest : colsLength - 1 - diagonalRest;
            int row = (column < colsLength) ? rowsLength - 1 - diagonalRest : rowsLength - 1 - diagonalRest - (column - rowsLength);
            return new int[] { row, realColumn };
        }

        public int[] GetBoard()
        {
            return boardLine;
        }
    }
}
