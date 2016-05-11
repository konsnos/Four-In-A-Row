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
        /// Returns the board in a single dimensional array by a certain pattern.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        int[] GetBoard(Board board);
        /// <summary>
        /// Returns the row and column of an index from the single dimensinal array this class generated.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int[] GetPos(int index, int rowsLength, int colsLength);
    }
}
