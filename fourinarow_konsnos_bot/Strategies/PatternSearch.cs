using System;

namespace FourInARow.Strategies
{
    class PatternSearch
    {
        Random r;

        int[][] patterns;

        public PatternSearch()
        {
            r = new Random();
        }

        /// <summary>
        /// Calculate the next move.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public int NextMove(Board board)
        {
            int col;
            col = checkColsForWins(board);
            if (col == -1)
            {
                do
                {
                    col = r.Next(board.ColsLength);
                } while (!board.CheckIfValid(col));
            }
            return col;
        }

        private int checkColsForWins(Board board)
        {
            //findColOfImminentWin(board);

            return -1;
        }
    }
}