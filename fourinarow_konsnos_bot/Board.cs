using System;
using System.Text;

namespace FourInARow
{
    public class Board
    {
        /// <summary>Board begins from top left. Jagged array is [Row][Column]. </summary>
        public int[][] BoardArray { get; private set; }
        ///<summary>Game round. Counting player and opponent moves. Player 1 plays first.</summary>
        public int GameRound { get; private set; }
        /// <summary>Id of the player's bot. </summary>
        public int MyBotId { get; private set; }
        /// <summary> Columns length. </summary>
        public int ColsLength {get;private set;}
        /// <summary> Rows column. Starts from top. </summary>
        public int RowsLength {get;private set;}
        /// <summary> Heights of columns. </summary>
        public int[] ColsHeights { get; private set; }

        public const int COLUMN_FULL = -1;

        public Board()
        {
            // Initialise variables.
            ColsLength = 0;
            RowsLength = 0;
        }

        public void SetMyBotId(int myBotId)
        {
            MyBotId = myBotId;
        }

        public void SetColumnsLength(int newColsLength)
        {
            ColsLength = newColsLength;
            // Initialize column heights.
            ColsHeights = new int[ColsLength];
        }

        public void SetRowsLength(int newRowsLength)
        {
            RowsLength = newRowsLength;
        }

        public void UpdateGameRound(int newGameRound)
        {
            GameRound = newGameRound;
        }

        public void Update(int[][] boardArray)
        {
            BoardArray = boardArray;

            // Calculate all column heights
            for (int c = 0; c < ColsLength; c++)
                ColsHeights[c] = getColumnHeight(c);

            if (GlobalVars.PRINT_DEBUG)
            {
                Console.WriteLine();
                Console.WriteLine(ToString());
            }
        }
        
        /// <summary>
        /// Checks the next free row in a column and returns it. Returns the length of the column if full.
        /// </summary>
        /// <returns>-1 if column is full.</returns>
        private int getColumnHeight(int col)
        {
            for (int row = RowsLength - 1; row > -1; row--)
            {
                if(GetState(col, row) == FieldState.Free)
                    return row;
            }
            
            return COLUMN_FULL;
        }


        
        /// <summary>
        /// Checks the state of a field.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns>FieldState enum of Free, Me, or Opponent.</returns>
        public FieldState GetState(int col, int row)
        {
            if (BoardArray[row][col] == 0)
                return FieldState.Free;
            if (BoardArray[row][col] == MyBotId) 
                return FieldState.Me;
            return FieldState.Opponent;
        }

        ///<summary>
        /// Checks if a column to drop has a valid movement.
        /// </summary>
        /// <returns>True if the move is valid. False if not.</returns>
        public bool CheckIfValid(int dropCol)
        {
            if (getColumnHeight(dropCol) == -1)
            {
                if (GlobalVars.PRINT_DEBUG)
                    Console.WriteLine("Invalid move at col {0} rejected", dropCol);
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < RowsLength; i++)
            {
                for (int j = 0; j < ColsLength; j++)
                {
                    sb.Append(BoardArray[i][j]).Append(" ");
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}