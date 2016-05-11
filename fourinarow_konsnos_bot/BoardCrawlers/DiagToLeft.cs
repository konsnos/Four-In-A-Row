using System;

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
        public int[] GetBoard(Board board)
        {
            int[] diags = new int[(board.RowsLength * board.ColsLength) + board.RowsLength + board.ColsLength - 1];

            int count = 0;
            for(int c = 0;c<board.ColsLength;c++)
            {
                for(int r = board.RowsLength - 1;r>-1;r--)
                {
                    int c_b = c - (board.RowsLength - r - 1);
                    if (c_b > -1)
                        diags[count++] = board.BoardArray[r][c_b];
                    else
                        break;
                }
                diags[count++] = int.MaxValue;
            }

            int maxC = board.ColsLength - 1;
            for(int r = board.RowsLength - 2;r>-1;r--)
            {
                for(int r_b = r; r_b > -1;r_b--)
                {
                    int c = maxC - (r-r_b);
                    if (c > -1)
                        diags[count++] = board.BoardArray[r_b][c];
                    else
                        break;
                }
                diags[count++] = int.MaxValue;
            }

            return diags;
        }

        public int[] GetPos(int index, int rowsLength, int colsLength)
        {
            int count = 0;
            for (int c = 0; c < colsLength; c++)
            {
                for (int r = rowsLength - 1; r > -1; r--)
                {
                    int c_b = c - (rowsLength - r - 1);
                    if (c_b > -1)
                    {
                        if (++count == index)
                            return new int[] { r, c_b };
                    }
                    else
                        break;
                }
                count++;
            }

            int maxC = colsLength - 1;
            for (int r = rowsLength - 2; r > -1; r--)
            {
                for (int r_b = r; r_b > -1; r_b--)
                {
                    int c = maxC - (r - r_b);
                    if (c > -1)
                    {
                        if (++count == index)
                            return new int[] { r_b, c };
                    }
                    else
                        break;
                }
                count++;
            }

            throw new IndexOutOfRangeException("Index " + index + " is out of range.");
        }
    }
}
