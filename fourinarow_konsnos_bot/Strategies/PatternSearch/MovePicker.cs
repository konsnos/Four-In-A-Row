using System;
using System.Collections.Generic;

namespace FourInARow.Strategies.PatternSearch
{
    class MoveProbabilities
    {
        private float[] probabilities;
        private int[] lowestAssignmentAbsolute;

        private const float PROB_SURE_WIN = 11f;
        private const float PROB_SURE_MOVE = 10f;
        private const float PROB_SURE_AVOID = -10f;

        public MoveProbabilities(int columnLength)
        {
            probabilities = new float[columnLength];
            lowestAssignmentAbsolute = new int[columnLength];
        }

        /// <summary>
        /// Search for the highest probability to pick as move.
        /// </summary>
        /// <returns></returns>
        public int GetBestMove()
        {
            if (GlobalVars.PRINT_DEBUG)
            {
                Console.WriteLine("Probabilities");

                for (int c = 0; c < probabilities.Length; c++)
                    Console.Write("{0},", probabilities[c]);
                Console.WriteLine();
            }

            int bestColumn = -1;

            for (int c = 0; c < probabilities.Length; c++)
            {
                if (bestColumn == -1)
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

        /// <summary>
        /// Resets lowest assignments and probabilities.
        /// </summary>
        /// <param name="board"></param>
        public void Reset(Board board)
        {
            for (int c = 0; c < board.ColsLength; c++)
            {
                if (board.ColsHeights[c] == Board.COLUMN_FULL)  // If column is full
                {
                    lowestAssignmentAbsolute[c] = board.RowsLength;    // Don't allow further calculation of this row.
                    probabilities[c] = PROB_SURE_AVOID;
                }
                else
                {
                    lowestAssignmentAbsolute[c] = -1;
                    probabilities[c] = 0;
                }
            }
        }

        public void UpdateImportants(Board board, List<int[]> playerPoss, List<int[]> opponentPoss)
        {
            const float PROB_IMPORTANT = 0.8f;

            int[] lowestAssignment = new int[board.ColsLength];
            for (int i = lowestAssignment.Length - 1; i > -1; i--)
                lowestAssignment[i] = -1;

            for (int c = 0;c<board.ColsLength;c++)
            {
                if(probabilities[c] < PROB_SURE_MOVE && probabilities[c] > PROB_SURE_AVOID)
                {
                    foreach(int[] pos in playerPoss)
                    {
                        if (pos[1] == c)
                        {
                            int difference = board.ColsHeights[c] - pos[0];
                            if (difference == 0)
                            {
                                probabilities[c] += PROB_IMPORTANT;
                                lowestAssignment[c] = pos[0];
                            }
                            else if (difference == 1)
                            {
                                probabilities[c] -= PROB_IMPORTANT;
                                lowestAssignment[c] = pos[0];
                            }
                        }
                    }
                    
                    foreach (int[] pos in opponentPoss)
                    {
                        if (pos[1] == c)
                        {
                            if (pos[0] > lowestAssignment[c])
                            {
                                int difference = board.ColsHeights[c] - pos[0];
                                if (difference == 0)
                                {
                                    probabilities[c] += PROB_IMPORTANT;
                                    lowestAssignment[c] = pos[0];
                                }
                                else if (difference == 1)
                                {
                                    probabilities[c] -= PROB_IMPORTANT;
                                    lowestAssignment[c] = pos[0];
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UdpateAbsolutes(Board board, List<int[]> absPPoss, List<int[]> absOPoss)
        {
            for (int c = 0; c < board.ColsLength; c++)
            {
                // Alter to win probabilities
                foreach (int[] pos in absPPoss)
                {
                    if (pos[1] == c) // if in same column
                    {
                        if (pos[0] > lowestAssignmentAbsolute[c])   // if more important than last found.
                        {
                            int difference = board.ColsHeights[c] - pos[0];
                            if (difference == 0)
                            {
                                probabilities[c] = PROB_SURE_WIN;   // Certain win!
                            }
                            else if (difference == 1)
                                probabilities[c] = PROB_SURE_AVOID;   // Avoid putting in here because the opponent will cover the win.
                            else
                                probabilities[c] += 0.2f;   // Let's reach it faster!
                            lowestAssignmentAbsolute[c] = pos[0];
                        }
                    }
                }

                // Alter to loose probabilities
                foreach (int[] pos in absOPoss)
                {
                    if (pos[1] == c) // if in same column
                    {
                        if (pos[0] > lowestAssignmentAbsolute[c])   // if more important than last found.
                        {
                            int difference = board.ColsHeights[c] - pos[0];
                            if (difference == 0)
                            {
                                probabilities[c] = PROB_SURE_MOVE;   // Avoid certain loss
                                lowestAssignmentAbsolute[c] = pos[0];
                            }
                            else if (difference == 1)
                            {
                                probabilities[c] = PROB_SURE_AVOID;   // Avoid putting in here because the opponent will have a certain win.
                                lowestAssignmentAbsolute[c] = pos[0];
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Randomise choices.
        /// This should only affect a little.
        /// </summary>
        public void RandomizeProbabilities()
        {
            Random r = new Random();
            const int probabilityAmount = 3000;
            const float turnToFloat = 10000f;

            for (int c = 0; c < probabilities.Length; c++)
            {
                if (probabilities[c] < PROB_SURE_MOVE && probabilities[c] > PROB_SURE_AVOID)
                {
                    probabilities[c] += (r.Next(probabilityAmount) / turnToFloat); // May affect it from 0 to 0.3
                }
            }
        }
    }
}
