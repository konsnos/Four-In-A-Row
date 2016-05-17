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
        private IBoardCrawler diagToRight;
        private IBoardCrawler diagToLeft;

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

            offsetIndexes = new int[] { 0, 1, 2, 3, 1, 3 };
        }

        public void UpdateBoardSize(Board board)
        {
            if(board.ColsLength != 0 && board.RowsLength != 0)
            {
                rows = new Rows(board.RowsLength, board.ColsLength);
                columns = new Columns(board.RowsLength, board.ColsLength);
                diagToRight = new DiagToRight(board.RowsLength, board.ColsLength);
                diagToLeft = new DiagToLeft(board.RowsLength, board.ColsLength);

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
            diagToRight.CreateBoard(board);
            diagToLeft.CreateBoard(board);

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
            
            // Get boards to traverse.
            int[] rowsBoard = rows.GetBoard();
            int[] colsBoard = columns.GetBoard();
            int[] diagToRightBoard = diagToRight.GetBoard();
            int[] diagToLeftBoard = diagToLeft.GetBoard();

            if(GlobalVars.PRINT_DEBUG)
            {
                Console.WriteLine();

                Console.WriteLine("Rows");
                Console.WriteLine(string.Join("", rowsBoard));

                Console.WriteLine("Columns");
                Console.WriteLine(string.Join("", colsBoard));

                Console.WriteLine("Diag to Right");
                Console.WriteLine(string.Join("", diagToRightBoard));

                Console.WriteLine("Diag to Left");
                Console.WriteLine(string.Join("", diagToLeftBoard));

                Console.WriteLine();
            }

            for(int i = 0;i<winPatterns.Length;i++)
            {
                foreach (int pos in rowsBoard.Locate(winPatterns[i]))
                {
                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = rows.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Win found from row at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);

                    return boardPos[1];
                }

                foreach (int pos in colsBoard.Locate(winPatterns[i]))
                {
                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = columns.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Win found from column at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);

                    return boardPos[1];
                }

                foreach(int pos in diagToRightBoard.Locate(winPatterns[i]))
                {

                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = diagToRight.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Win found from diag to right at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);

                    return boardPos[1];
                }

                foreach (int pos in diagToLeftBoard.Locate(winPatterns[i]))
                {
                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = diagToLeft.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Win found from diag to right at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);

                    return boardPos[1];
                }
            }

            for (int i = 0; i < lossPatterns.Length; i++)
            {
                foreach (int pos in rowsBoard.Locate(lossPatterns[i]))
                {
                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = rows.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Loss found from row at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);

                    return boardPos[1];
                }

                foreach (int pos in colsBoard.Locate(lossPatterns[i]))
                {
                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = columns.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Loss found from column at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);

                    return boardPos[1];
                }
                
                foreach (int pos in diagToRightBoard.Locate(lossPatterns[i]))
                {
                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = diagToRight.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);
                    
                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Loss found from diag to right at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);

                    return boardPos[1];
                }

                foreach (int pos in diagToLeftBoard.Locate(lossPatterns[i]))
                {
                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = diagToLeft.GetPos(offsettedIndex, board.RowsLength, board.ColsLength);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Loss found from diag to left at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);

                    return boardPos[1];
                }
            }

            return -1;
        }
    }
}