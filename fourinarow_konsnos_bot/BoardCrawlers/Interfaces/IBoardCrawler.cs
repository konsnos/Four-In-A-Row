using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInARow.BoardCrawlers
{
    interface IBoardCrawler
    {
        /// <summary>
        /// Creates the board in a single dimensional array by a certain pattern.
        /// </summary>
        /// <param name="board"></param>
        void CreateBoard(Board board);
        /// <summary>
        /// Returns the created board.
        /// </summary>
        /// <returns></returns>
        int[] GetBoard();
        /// <summary>
        /// Returns the row and column of an index from the single dimensinal array this class generated.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>A 2D array of [row, column].</returns>
        int[] GetPos(int index, int rowsLength, int colsLength);
    }
}
