using System;

namespace FourInARow.Strategies
{
    public class BasicStrategy : IStrategy
    {
        Random r;
        
        public BasicStrategy()
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
            if(col == -1)
            {
                do
                {
                    col = r.Next(board.ColsLength);
                } while (!board.CheckIfValid(col));
            }
            return col;
        }

        /// <summary>
        /// Checks every column if me or the opponent will win.
        /// </summary>
        /// <param name="board"></param>
        /// <returns>Column to place for a win or avoid loss. -1 if there is no imminent win or loss.</returns>
        private int checkColsForWins(Board board)
        {
            for (int c = 0; c < board.ColsLength; c++)
            {
                int r = board.GetColumnHeight(c);
                if(r != -1)
                {
                    if(checkWinMove(board, FieldState.Me, r, c))
                    {
                        if(GlobalVars.PRINT_DEBUG)
                            Console.WriteLine("Win at {0} column", c);
                        return c;
                    }
                    
                    if(checkWinMove(board, FieldState.Opponent, r, c))
                    {
                        if (GlobalVars.PRINT_DEBUG)
                            Console.WriteLine("Block at {0} column", c);
                        return c;
                    }
                }
            }
            
            return -1;
        }
        
        /// <summary>
        /// Checks if a player will win with a move at a certain position.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="fs"></param>
        /// <param name="row">Row starts from top.</param>
        /// <param name="col"></param>
        /// <returns>True to win.False if not.</returns>
        private bool checkWinMove(Board board, FieldState fs, int row, int col)
        {
            int straightLine;
            // Check left and right.
            straightLine = 1;
            {
                bool continueLeft = true;
                bool continueRight = true;
                for (int i = 1;continueLeft || continueRight;i++)
                {
                    if(continueLeft)
                    {
                        if(col - i > -1)
                        {
                            if(board.GetState(col-i, row) == fs)
                                straightLine++;
                            else
                                continueLeft = false;
                        }
                        else
                            continueLeft = false;
                    }
                    
                    if(continueRight)
                    {
                        if(col + i < board.ColsLength)
                        {
                            if(board.GetState(col+i, row) == fs)
                                straightLine++;
                            else
                                continueRight = false;
                        }
                        else
                            continueRight = false;
                    }
                    
                    if(straightLine > 3)
                        return true;
                }
            }
            
            // Check top and bottom
            straightLine = 1;
            {
                bool continueTop = true;
                bool continueBottom = true;
                for (int i = 1; continueTop || continueBottom; i++)
                {
                    if(continueBottom)
                    {
                        if(row+i<board.RowsLength)
                        {
                            if(board.GetState(col, row+i) == fs)
                                straightLine++;
                            else
                                continueBottom = false;
                        }
                        else
                            continueBottom = false;
                    }
                    
                    if (continueTop)
                    {
                        if(row - i>-1)
                        {
                            if(board.GetState(col, row-i) == fs)
                                straightLine++;
                            else
                                continueTop = false;
                        }
                        else
                            continueTop = false;
                    }
                    
                    if(straightLine > 3)
                        return true;
                }
            }

            // Check top left to bottom right
            straightLine = 1;
            {
                bool continueTopLeft = true;
                bool continueBottomRight = true;
                for(int i = 1; continueTopLeft || continueBottomRight;i++)
                {
                    if(continueTopLeft)
                    {
                        if (row - i > -1 && col - i > -1)
                        {
                            if (board.GetState(col - i, row - i) == fs)
                                straightLine++;
                            else
                                continueTopLeft = false;
                        }
                        else
                            continueTopLeft = false;
                    }

                    if(continueBottomRight)
                    {
                        if (row + i < board.RowsLength && col + i < board.ColsLength)
                        {
                            if (board.GetState(col + i, row + i) == fs)
                                straightLine++;
                            else
                                continueBottomRight = false;

                        }
                        else
                            continueBottomRight = false;
                    }

                    if (straightLine > 3)
                        return true;
                }
            }

            // Check top right to bottom left
            straightLine = 1;
            {
                bool continueTopRight = true;
                bool continueBottomLeft = true;
                for(int i = 1;continueTopRight || continueBottomLeft;i++)
                {
                    if(continueTopRight)
                    {
                        if (col + i < board.ColsLength && row - i > -1)
                        {
                            if (board.GetState(col + i, row - i) == fs)
                                straightLine++;
                            else
                                continueTopRight = false;
                        }
                        else
                            continueTopRight = false;
                    }

                    if(continueBottomLeft)
                    {
                        if (col - i > -1 && row + i < board.RowsLength)
                        {
                            if (board.GetState(col - i, row + i) == fs)
                                straightLine++;
                            else
                                continueBottomLeft = false;
                        }
                        else
                            continueBottomLeft = false;
                    }

                    if (straightLine > 3)
                        return true;
                }
            }
            
            return false;
        }
    }
}