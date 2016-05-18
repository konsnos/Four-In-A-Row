using FourInARow.BoardCrawlers;
using System;
using System.Collections.Generic;

namespace FourInARow.Strategies.PatternSearch
{
    class Patterns
    {
        /** Player's id. */
        private int id;
        private static bool initialized = false;

        /***** ABSOLUTE PRIORITY ****/
        /** Patterns that guarantee victory. */
        public int[][] AbsolutePatterns { get; private set; }
        /** OFfset for the index that must be positioned. */
        public static int[] OffsetAbsolutePatterns { get; private set; }

        /******* IMPORTANT PRIORITY *****/
        public int[][] ImportantPatterns { get; private set; }
        public static int[] OffsetImportantPatterns { get; private set; }

        public Patterns(int playerid)
        {
            id = playerid;

            AbsolutePatterns = new int[][]
            {
                new int[] { 0, id, id, id },
                new int[] { id, 0, id, id },
                new int[] { id, id, 0, id },
                new int[] { id, id, id, 0 }
            };

            ImportantPatterns = new int[][]
            {
                new int[] { 0, 0, id, id, 0 },
                new int[] { 0, id, id, 0, 0 },
                new int[] { 0, id, 0, id, 0 }
            };

            if (!initialized)
            {
                initialized = true;
                OffsetAbsolutePatterns = new int[] { 0, 1, 2, 3 };

                OffsetImportantPatterns = new int[] { 1, 3, 2 };
            }
        }

        /// <summary>
        /// Searches for positions that can win the game.
        /// </summary>
        public List<int[]> GetAbsolutePositions(IBoardCrawler rows, IBoardCrawler columns, IBoardCrawler diagToRight, IBoardCrawler diagToLeft, int rowsLength, int colsLength)
        {
            List<int[]> moves = new List<int[]>();

            for (int i = 0; i < AbsolutePatterns.Length; i++)
            {
                moves.AddRange(getPatternMatches(rows, AbsolutePatterns, OffsetAbsolutePatterns, i, rowsLength, colsLength));
                moves.AddRange(getPatternMatches(columns, AbsolutePatterns, OffsetAbsolutePatterns, i, rowsLength, colsLength));
                moves.AddRange(getPatternMatches(diagToRight, AbsolutePatterns, OffsetAbsolutePatterns, i, rowsLength, colsLength));
                moves.AddRange(getPatternMatches(diagToLeft, AbsolutePatterns, OffsetAbsolutePatterns, i, rowsLength, colsLength));
            }

            return moves;
        }

        /// <summary>
        /// Searches for positions that can win the game.
        /// </summary>
        public List<int[]> GetImportantPositions(IBoardCrawler rows, IBoardCrawler columns, IBoardCrawler diagToRight, IBoardCrawler diagToLeft, int rowsLength, int colsLength)
        {
            List<int[]> moves = new List<int[]>();

            for (int i = 0; i < ImportantPatterns.Length; i++)
            {
                moves.AddRange(getPatternMatches(rows, ImportantPatterns, OffsetImportantPatterns, i, rowsLength, colsLength));
                moves.AddRange(getPatternMatches(columns, ImportantPatterns, OffsetImportantPatterns, i, rowsLength, colsLength));
                moves.AddRange(getPatternMatches(diagToRight, ImportantPatterns, OffsetImportantPatterns, i, rowsLength, colsLength));
                moves.AddRange(getPatternMatches(diagToLeft, ImportantPatterns, OffsetImportantPatterns, i, rowsLength, colsLength));
            }

            return moves;
        }

        /// <summary>
        /// Searches for patterns in a board and returns a list of the positions found.
        /// </summary>
        private List<int[]> getPatternMatches(IBoardCrawler board, int[][] patterns, int[] offsets, int patternIndex, int rowsLength, int colsLength)
        {
            List<int[]> matches = new List<int[]>();

            foreach(int pos in board.GetBoard().Locate(patterns[patternIndex]))
            {
                int offsettedIndex = offsets[patternIndex] + pos;
                int[] boardPos = board.GetPos(offsettedIndex, rowsLength, colsLength);
                matches.Add(boardPos);

                if (GlobalVars.PRINT_DEBUG)
                    Console.WriteLine("Absolute move found at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);
            }

            return matches;
        }

        /// <summary>
        /// Searches for patterns in a board and returns a list of the positions found.
        /// </summary>
        private List<int[]> getPatternMatches(IBoardCrawler board, int[][] patterns, List<int>[] offsets, int patternIndex, int rowsLength, int colsLength)
        {
            List<int[]> matches = new List<int[]>();

            foreach (int pos in board.GetBoard().Locate(patterns[patternIndex]))
            {
                foreach(int offset in offsets[patternIndex])
                {
                    int offsettedIndex = offset + pos;
                    int[] boardPos = board.GetPos(offsettedIndex, rowsLength, colsLength);
                    matches.Add(boardPos);

                    if (GlobalVars.PRINT_DEBUG)
                        Console.WriteLine("Important move found at index: {0} and row,column: {1},{2}", pos, boardPos[0], boardPos[1]);
                }
            }

            return matches;
        }
    }
}
