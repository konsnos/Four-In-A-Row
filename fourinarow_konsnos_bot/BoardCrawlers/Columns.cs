using FourInARow.Strategies;

namespace FourInARow.BoardCrawlers
{
    /// <example>
    /// Rows: 2
    /// Columns: 5
    /// 0,3,6,9 ,12
    /// 1,4,7,10,13
    /// 2,5,8,11,14
    /// </example>
    class Columns : IBoardCrawler
    {
        public int[] boardLine { get; private set; }

        public Columns(int rowsLength, int colsLength)
        {
            boardLine = new int[rowsLength * (colsLength + 1)];

            for(int r = rowsLength;r<boardLine.Length;r+= (rowsLength + 1))
                boardLine[r] = PatternSearchStrategy.PATTERN_BREAKER; // new lines
        }

        public void CreateBoard(Board board)
        {
            int count = 0;
            for (int c = 0; c < board.ColsLength; c++)
            {
                for (int r = 0; r < board.RowsLength; r++)
                    boardLine[count++] = board.BoardArray[r][c];
                count++;    // new line is assigned
            }
        }

        public int[] GetPos(int index, int rowsLength, int colsLength)
        {
            return new int[] { index % (rowsLength + 1), index / (rowsLength + 1)};
        }

        public int[] GetBoard()
        {
            return boardLine;
        }
    }
}
