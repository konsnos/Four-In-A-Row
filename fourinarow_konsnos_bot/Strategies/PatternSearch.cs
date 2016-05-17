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

        private float[] probabilities;

        private const float PROB_SURE_MOVE = 2f;
        private const float PROB_SURE_AVOID = -2f;

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

                probabilities = new float[board.ColsLength];

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

            for (int i = 0; i < probabilities.Length; i++)
                probabilities[i] = 0;

            rows.CreateBoard(board);
            columns.CreateBoard(board);
            diagToRight.CreateBoard(board);
            diagToLeft.CreateBoard(board);

            // Get boards to traverse
            int[] rowsBoard = rows.GetBoard();
            int[] colsBoard = columns.GetBoard();
            int[] diagToRightBoard = diagToRight.GetBoard();
            int[] diagToLeftBoard = diagToLeft.GetBoard();
            
            if (GlobalVars.PRINT_DEBUG)
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

            if (GlobalVars.PRINT_DEBUG)
                Console.WriteLine("Check for wins");
            List<int[]> winPositions = checkWinningPositions(winPatterns, rowsBoard, colsBoard, diagToRightBoard, diagToLeftBoard, board.RowsLength, board.ColsLength);
            if (GlobalVars.PRINT_DEBUG)
                Console.WriteLine("Check for losses");
            List<int[]> lossPositions = checkWinningPositions(lossPatterns, rowsBoard, colsBoard, diagToRightBoard, diagToLeftBoard, board.RowsLength, board.ColsLength);

            udpateProbabilities(board, winPositions, lossPositions);
            randomizeProbabilities();
            col = getBestMove();

            if (col == -1)
            {
                do
                {
                    col = r.Next(board.ColsLength);
                } while (!board.CheckIfValid(col));
            }
            return col;
        }

        /// <summary>
        /// Search for the highest probability to pick as move.
        /// </summary>
        /// <returns></returns>
        private int getBestMove()
        {
            int bestColumn = -1;

            for(int c = 0;c<probabilities.Length;c++)
            {
                if(bestColumn == -1)
                {
                    if (probabilities[c] >= 0f)
                        bestColumn = c;
                }
                else
                {
                    if (probabilities[c] > probabilities[bestColumn])
                        bestColumn = c;
                }
            }

            return bestColumn;
        }


        private void randomizeProbabilities()
        {
            const int probabilityAmount = 3000;
            const float turnToFloat = 10000f;

            for(int c = 0;c<probabilities.Length;c++)
            {
                if(probabilities[c] != PROB_SURE_MOVE && probabilities[c] != PROB_SURE_AVOID)
                {
                    probabilities[c] += (r.Next(probabilityAmount) / turnToFloat); // May affect it from 0 to 0.3
                }
            }
        }

        private void udpateProbabilities(Board board, List<int[]> winPoss, List<int[]> lossPoss)
        {
            int[] loswestAssignment = new int[board.ColsLength];
            for (int c = 0; c < board.ColsLength; c++)
            {
                if (board.ColsHeights[c] == Board.COLUMN_FULL)  // If column is full
                {
                    loswestAssignment[c] = board.RowsLength;    // Don't allow further calculation of this row.
                    probabilities[c] = PROB_SURE_AVOID;
                }
                else
                    loswestAssignment[c] = 0;
            }

            for(int c = 0;c<board.ColsLength;c++)
            {
                // Alter to win probabilities
                foreach(int[] pos in winPoss)
                {
                    if(pos[1] == c) // if in same column
                    {
                        if(pos[0] > loswestAssignment[c])   // if more important than last found.
                        {
                            int difference = board.ColsHeights[c] - pos[0];
                            if (difference == 0)
                                probabilities[c] = PROB_SURE_MOVE;   // Certain win!
                            else if (difference == 1)
                                probabilities[c] = PROB_SURE_AVOID;   // Avoid putting in here because the opponent will cover the win.
                            else
                                probabilities[c] = 0.2f;
                            loswestAssignment[c] = pos[0];
                        }
                    }
                }

                // Alter to loose probabilities
                foreach(int[] pos in lossPoss)
                {
                    if(pos[1] == c) // if in same column
                    {
                        if(pos[0] > loswestAssignment[c])   // if more important than last found.
                        {
                            int difference = board.ColsHeights[c] - pos[0];
                            if (difference == 0)
                                probabilities[c] = PROB_SURE_MOVE;   // Avoid certain loss
                            else if (difference == 1)
                                probabilities[c] = PROB_SURE_AVOID;   // Avoid putting in here because the opponent will have a certain win.
                            else
                                probabilities[c] = 0f;
                            loswestAssignment[c] = pos[0];
                        }
                    }
                }
            }

            if(GlobalVars.PRINT_DEBUG)
            {
                Console.WriteLine("Probabilities");

                for (int c = 0; c < board.ColsLength; c++)
                    Console.Write("{0},", probabilities[c]);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Checks all the rows given for the pattern given and returns the winning moves.
        /// </summary>
        /// <param name="patterns"></param>
        /// <param name="rowsBoard"></param>
        /// <param name="colsBoard"></param>
        /// <param name="diagToRightBoard"></param>
        /// <param name="diagToLeftBoard"></param>
        /// <param name="rowsLength"></param>
        /// <param name="colsLength"></param>
        /// <returns></returns>
        private List<int[]> checkWinningPositions(int[][] patterns, int[] rowsBoard, int[] colsBoard, int[] diagToRightBoard, int[] diagToLeftBoard, int rowsLength, int colsLength)
        {
            List<int[]> winningMoves = new List<int[]>();
            int[] boardPos;
            int offsettedIndex;

            for(int i = 0;i< patterns.Length;i++)
            {
                foreach (int pos in rowsBoard.Locate(patterns[i]))
                {
                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = rows.GetPos(offsettedIndex, rowsLength, colsLength);
                    winningMoves.Add(boardPos);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Win found from row at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);
                }

                foreach (int pos in colsBoard.Locate(patterns[i]))
                {
                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = columns.GetPos(offsettedIndex, rowsLength, colsLength);
                    winningMoves.Add(boardPos);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Win found from column at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);
                }

                foreach(int pos in diagToRightBoard.Locate(patterns[i]))
                {

                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = diagToRight.GetPos(offsettedIndex, rowsLength, colsLength);
                    winningMoves.Add(boardPos);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Win found from diag to right at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);
                }

                foreach (int pos in diagToLeftBoard.Locate(patterns[i]))
                {
                    offsettedIndex = offsetIndexes[i] + pos;
                    boardPos = diagToLeft.GetPos(offsettedIndex, rowsLength, colsLength);
                    winningMoves.Add(boardPos);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Win found from diag to right at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);
                }
            }

            return winningMoves;
        }
    }
}