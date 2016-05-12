using System;

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
        public int[] BoardLine { get; private set; }
        /// <summary>
        /// Initial row to start assigning indexes.
        /// </summary>
        private int initR = 3;
        private int lastCol;
        /// <summary>
        /// The line length of each diagonal. Each line should always reach this length in order to easily calculate the position of an index.
        /// </summary>
        private int lineLength;

        public DiagToRight(int rowsLength, int colsLength)
        {
            lastCol = colsLength - 3;
            lineLength = rowsLength + 1;
            BoardLine = new int[((rowsLength - initR) + (colsLength - lastCol) - 1) * lineLength];
        }

        public void CreateBoard(Board board)
        {
            int count = 0;
            for(int r = initR;r<board.RowsLength;r++)
            {
                int curLineLength = 0;
                for(int c = 0;c<board.ColsLength;c++)
                {
                    int r_b = r - c;
                    if (r_b > -1)
                    {
                        BoardLine[count++] = board.BoardArray[r_b][c];
                        curLineLength++;
                    }
                    else
                    {
                        break;
                    }
                }

                for (; curLineLength < lineLength; curLineLength++)
                    BoardLine[count++] = int.MaxValue;  // Fill up line length

                BoardLine[count++] = int.MaxValue;
            }

            int maxR = board.RowsLength - 1;
            for(int c = 1;c<lastCol;c++)
            {
                int curLineLength = 0;
                for(int c_b = c;c_b<board.ColsLength;c_b++)
                {
                    int r_b = maxR - (c-c_b);
                    if (r_b > -1)
                        BoardLine[count++] = board.BoardArray[r_b][c_b];
                    else
                        break;
                }
                for (; curLineLength < lineLength; curLineLength++)
                    BoardLine[count++] = int.MaxValue;  // Fill up line length

                BoardLine[count++] = int.MaxValue;
            }
        }


        public int[] GetPos(int index, int rowsLength, int colsLength)
        {


            /*int count = 0;
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

            throw new IndexOutOfRangeException("Index " + index + " is out of range.");*/
        }
    }
}
