using System;

namespace FourInARow
{

    public interface IStrategy
    {
        int NextMove(Board board);
    }

    public class Strategy : IStrategy
    {
        Random r;
        
        public Strategy()
        {
            r = new Random();
        }
        
        /**
         *  AI here.
         **/
        public int NextMove(Board board)
        {
            int col;
            col = checkColsForWins(board);
            if(col == -1)
            {
                do
                {
                    col = r.Next(board.ColsLength);
                } while (!checkIfValid(board, col));
            }
            return col;
        }
        
        /**
         * Checks every column if me or the opponent will win.
         * @returns Column to place for a win or avoid loss. -1 if there is no imminent win or loss. 
         **/
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
        
        /**
         * Checks if a player will win with a move at a certain position.
         * @returns  True to win. False if not.
         **/
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
                            if(board.State(col-i, row) == fs)
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
                            if(board.State(col+i, row) == fs)
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
                            if(board.State(col, row+i) == fs)
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
                            if(board.State(col, row-i) == fs)
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
            
            //TODO: Check diagonal
            
            return false;
        }
        
        /**
         * Checks if a column to drop has a valid movement.
         * @return True if the move is valid. False if not.
         **/
        private bool checkIfValid(Board board, int dropCol)
        {
            if(board.GetColumnHeight(dropCol) == -1)
            {
                if (GlobalVars.PRINT_DEBUG)
                    Console.WriteLine("Invalid move at col {0} rejected", dropCol);
                return false;
            }
            return true;
        }
    }

}