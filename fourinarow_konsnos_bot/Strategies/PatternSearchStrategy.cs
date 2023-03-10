using FourInARow.BoardCrawlers;
using FourInARow.Strategies.PatternSearch;
using System;
using System.Collections.Generic;

namespace FourInARow.Strategies
{
    class PatternSearchStrategy : IStrategy
    {
        static Random r;

        static Patterns playerPatterns;
        static Patterns opponentPatterns;
        MoveProbabilities movePicker;

        static private IBoardCrawler rows;
        static private IBoardCrawler columns;
        static private IBoardCrawler diagToRight;
        static private IBoardCrawler diagToLeft;

        private float[] probabilities;

        private const float PROB_SURE_MOVE = 2f;
        private const float PROB_SURE_AVOID = -2f;

        ///<summary>Id to use to break the lines in the crawlers to indicate a new line.</summary>
        public const int PATTERN_BREAKER = 9;

        public PatternSearchStrategy()
        {
            if(r == null)
                r = new Random();
        }

        /// <summary>
        /// Create patterns of win and loss.
        /// </summary>
        /// <param name="newMyBotId"></param>
        public void UpdateSelfBotId(int newMyBotId)
        {
            playerPatterns = new Patterns(newMyBotId);
            opponentPatterns = new Patterns((newMyBotId == 1) ? 2 : 1);
        }

        /// <summary>
        /// If both columns and rows are set then board crawlers, and move probabilities are initialised.
        /// </summary>
        /// <param name="board"></param>
        public void UpdateBoardSize(Board board)
        {
            if(board.ColsLength != 0 && board.RowsLength != 0)
            {
                rows = new Rows(board.RowsLength, board.ColsLength);
                columns = new Columns(board.RowsLength, board.ColsLength);
                diagToRight = new DiagToRight(board.RowsLength, board.ColsLength);
                diagToLeft = new DiagToLeft(board.RowsLength, board.ColsLength);

                // as these are not static they should be initialized at a new instance of the class.
                probabilities = new float[board.ColsLength];
                movePicker = new MoveProbabilities(board.ColsLength);

                if (GlobalVars.PRINT_DEBUG)
                    Console.WriteLine("Crawlers initialised.");
            }
        }

        /// <summary>
        /// Calculate the next move.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public int NextMove(Board board)
        {
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

            List<int[]> playerPoss;
            List<int[]> opponentPoss;

            movePicker.Reset(board);

            if (GlobalVars.PRINT_DEBUG)
                Console.WriteLine("Check for player absolute");
            playerPoss = playerPatterns.GetAbsolutePositions(rows, columns, diagToRight, diagToLeft, board.RowsLength, board.ColsLength);
            if (GlobalVars.PRINT_DEBUG)
                Console.WriteLine("Check for opponent absolute");
            opponentPoss = opponentPatterns.GetAbsolutePositions(rows, columns, diagToRight, diagToLeft, board.RowsLength, board.ColsLength);

            movePicker.UdpateAbsolutes(board, playerPoss, opponentPoss);

            if (GlobalVars.PRINT_DEBUG)
                Console.WriteLine("Check for player important");
            playerPoss = playerPatterns.GetImportantPositions(rows, columns, diagToRight, diagToLeft, board.RowsLength, board.ColsLength);
            if (GlobalVars.PRINT_DEBUG)
                Console.WriteLine("Check for opponent important");
            opponentPoss = opponentPatterns.GetImportantPositions(rows, columns, diagToRight, diagToLeft, board.RowsLength, board.ColsLength);

            movePicker.UpdateImportants(board, playerPoss, opponentPoss);
            movePicker.UpdateColumnPriorities();

            if(GlobalVars.PRINT_DEBUG)
            {
                Console.WriteLine("True probabilities");
                movePicker.GetBestMove();
            }
            movePicker.RandomizeProbabilities();
            int col = movePicker.GetBestMove();

            // If no move found pick a random.
            if (col == -1)
            {
                do
                {
                    col = r.Next(board.ColsLength);
                } while (!board.CheckIfValid(col));
            }

            return col;
        }
    }
}