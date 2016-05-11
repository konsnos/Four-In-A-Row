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
        public int[] GetBoard(Board board)
        {
            int[] cols = new int[board.RowsLength * (board.ColsLength + 1)];

            int count = 0;
            for (int c = 0; c < board.ColsLength; c++)
            {
                for (int r = 0; r < board.RowsLength; r++)
                    cols[count++] = board.BoardArray[r][c];
                cols[count++] = int.MaxValue;    // new line
            }

            return cols;
        }

        public int[] GetPos(int index, int rowsLength, int colsLength)
        {
            return new int[] { index % (rowsLength + 1), index / (rowsLength + 1)};
        }
    }
}
