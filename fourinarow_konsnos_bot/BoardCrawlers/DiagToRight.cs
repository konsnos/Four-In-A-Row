﻿using System;

namespace FourInARow.BoardCrawlers
{
    /// <example>
    /// Rows: 6
    /// Columns: 7
    /// 	1	4	8	13	19	26	33
    /// 	0	3	7	12	18	25	32	39
    /// 	2	6	11	17	24	31	38	44
    /// 	5	10	16	23	30	37	43	50
    /// 	9	15	22	29	36	42	49	53
    /// 	14	21	28	35	41	48	52	55
    /// 	20	27	34	40	47	51	54	
    /// </example>
    class DiagToRight : IBoardCrawler
    {
        public int[] GetBoard(Board board)
        {
            int[] diags = new int[(board.RowsLength * board.ColsLength) + board.RowsLength + board.ColsLength - 1];

            int count = 0;
            for(int r = 0;r<board.RowsLength;r++)
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


        public int[] GetPos(int index, int rowsLength, int colsLength)
        {
            int count = 0;
            for (int r = 0; r < rowsLength; r++)
            {
                for (int c = 0; c < colsLength; c++)
                {
                    int r_b = r - c;
                    if (r_b > -1)
                    {
                        if (++count == index)
                            return new int[] { r_b, c };
                    }
                    else
                        break;
                }
                count++;
            }

            int maxR = rowsLength - 1;
            for (int c = 1; c < colsLength; c++)
            {
                for (int c_b = c; c_b < colsLength; c_b++)
                {
                    int r_b = maxR - (c - c_b);
                    if (r_b > -1)
                    {
                        if (++count == index)
                            return new int[] { r_b, c_b };
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
