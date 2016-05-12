using FourInARow.BoardCrawlers;
using System;
using System.Collections.Generic;

namespace FourInARow.Strategies
{
    class PatternSearch : IStrategy
    {
        Random r;

        int[][] winPatterns;
        int[][] lossPatterns;
        int[] offsetIndexes;

        private IBoardCrawler rows;
        private IBoardCrawler columns;

        public PatternSearch()
        {
            r = new Random();
        }

        /// <summary>
        /// Create patterns of win and loss.
        /// </summary>
        /// <param name="newMyBotId"></param>
        public void UpdateSelfBotId(int newMyBotId)
        {
            winPatterns = createPatternWithId(newMyBotId);
            lossPatterns = createPatternWithId((newMyBotId == 1) ? 2 : 1);

            offsetIndexes = new int[] { 0, 1, 2, 3, 2, 3 };
        }

        public void UpdateBoardSize(Board board)
        {
            if(board.ColsLength != 0 && board.RowsLength != 0)
            {
                rows = new Rows(board.RowsLength, board.ColsLength);
                columns = new Columns(board.RowsLength, board.ColsLength);

                if (GlobalVars.PRINT_DEBUG)
                    Console.WriteLine("Crawlers initialised.");
            }
        }

        /// <summary>
        /// Create patterns of imminent win.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int[][] createPatternWithId(int id)
        {
            return new int[][]
            {
                new int[] { 0, id, id, id },
                new int[] { id, 0, id, id },
                new int[] { id, id, 0, id },
                new int[] { id, id, id, 0 },

                new int[] { 0, 0, id, id, 0 },  // Offset might have two possible indexes
                new int[] { 0, id, id, 0, 0 }   // Offset might have two possible indexes
            };
        }

        /// <summary>
        /// Calculate the next move.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public int NextMove(Board board)
        {
            int col;

            rows.CreateBoard(board);
            columns.CreateBoard(board);

            col = checkWinningMoves(board);
            if (col == -1)
            {
                do
                {
                    col = r.Next(board.ColsLength);
                } while (!board.CheckIfValid(col));
            }
            return col;
        }

        private int checkWinningMoves(Board board)
        {
            int[] boardPos;
            int offsettedIndex;
            for(int i = 0;i<winPatterns.Length;i++)
            {
                foreach (var pos in rows.Board.Locate(winPatterns[i]))
                {
                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Win found from row at index: " + pos);

                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = rows.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);
                    return boardPos[1];
                }

                foreach (var pos in columns.Board.Locate(winPatterns[i]))
                {
                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Win found from column at index: " + pos);

                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = columns.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);
                    return boardPos[1];
                }
            }

            for (int i = 0; i < lossPatterns.Length; i++)
            {
                foreach (var pos in rows.Board.Locate(lossPatterns[i]))
                {
                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Loss found from row at index: " + pos);

                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = rows.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);
                    return boardPos[1];
                }

                foreach (var pos in columns.Board.Locate(lossPatterns[i]))
                {
                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Loss found from column at index: " + pos);

                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = columns.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);
                    return boardPos[1];
                }
            }

            return -1;
        }
    }
}