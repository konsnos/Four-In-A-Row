using System;

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
        public int[] GetBoard(Board board)
        {
            int[] rows = new int[(board.RowsLength + 1) * board.ColsLength];

            int count = 0;
            for (int c = 0; c < board.RowsLength; c++)
            {
                for (int r = 0; r < board.ColsLength; r++)
                    rows[count++] = board.BoardArray[r][c];
                rows[count++] = int.MaxValue;    // new line
            }
            
            return rows;
        }

        public int[] GetPos(int index, int rowsLength, int colsLength)
        {
            return new int[] { index / (colsLength + 1), index % (colsLength + 1)};
        }
    }
}
