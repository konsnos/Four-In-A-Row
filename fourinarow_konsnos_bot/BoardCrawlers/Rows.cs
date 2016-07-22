using FourInARow.Strategies;

namespace FourInARow.BoardCrawlers
{
    /// <example>
    /// Rows: 2
    /// Columns: 5
    /// 0,1,2,3,4, 5
    /// 6,7,8,9,10,11
    /// </example>
    class Rows : IBoardCrawler
    {
        public int[] boardLine { get; private set; }

        public Rows(int rowsLength, int colsLength)
        {
            boardLine = new int[(rowsLength + 1) * colsLength];

            for(int c = colsLength;c<boardLine.Length;c+= (colsLength + 1))
                boardLine[c] = PatternSearchStrategy.PATTERN_BREAKER; // new lines
        }

        public void CreateBoard(Board board)
        {
            int count = 0;
            for (int r = 0; r < board.RowsLength; r++)
            {
                for (int c = 0; c < board.ColsLength; c++)
                    boardLine[count++] = board.BoardArray[r][c];
                count++;    // new line is already set
            }
        }

        public int[] GetPos(int index, int rowsLength, int colsLength)
        {
            return new int[] { index / (colsLength + 1), index % (colsLength + 1)};
        }

        public int[] GetBoard()
        {
            return boardLine;
        }
    }
}
