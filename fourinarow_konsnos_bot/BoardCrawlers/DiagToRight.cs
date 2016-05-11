using System;

namespace FourInARow.BoardCrawlers
{
    /// <example>
    /// Rows: 2
    /// Columns: 5
    /// 
    /// </example>
    class DiagToRight : IBoardCrawler
    {
        public int[] GetBoard(Board board)
        {
            int[] diags = new int[(board.RowsLength * board.ColsLength) + board.RowsLength + board.ColsLength - 2];

            int count = 0;
            for(int r =0;r<board.RowsLength;r++)
            {
                for(int c = 0;c<board.ColsLength;c++)
                {
                    int r_b = r - c;
                    if (r_b > -1)
                        diags[count++] = board.BoardArray[r_b][c];
                    else
                        break;
                }
                diags[count++] = int.MaxValue;
            }

            int maxR = board.RowsLength - 1;
            for(int c = 1;c<board.ColsLength;c++)
            {
                for(int c_b = c;c_b<board.ColsLength;c_b++)
                {
                    int r_b = maxR - (c-c_b);
                    if (r_b > -1)
                        diags[count++] = board.BoardArray[r_b][c_b];
                    else
                        break;
                }
                diags[count++] = int.MaxValue;
            }

            return diags;
        }

        public int[] GetPos(int index, int rowsLength, int colsLenght)
        {
            throw new NotImplementedException();
        }
    }
}
