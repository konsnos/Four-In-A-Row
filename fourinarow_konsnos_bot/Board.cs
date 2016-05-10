using System;
using System.Text;

namespace FourInARow
{
    public class Board
    {
        /** Board begins from top left. */
        private int[][] _boardArray;
        private int _mybotId;
        /** Columns */
        public int ColsLength {get;private set;}
        /** Rows */
        public int RowsLength {get;private set;}

        public void SetMyBotId(int myBotId)
        {
            _mybotId = myBotId;
        }

        public void Update(int[][] boardArray)
        {
            _boardArray = boardArray;
            ColsLength = _boardArray[0].Length;
            RowsLength = _boardArray.Length;

            if (GlobalVars.PRINT_DEBUG)
            {
                Console.WriteLine();
                Console.WriteLine(ToString());
            }
        }

        public int ColsNumber()
        {
            return _boardArray[0].Length;
        }
        
        /**
         * Checks the next free row in a column and returns it. Returns the length of the column if full.
         * @returns -1 if column is full.
         **/
        public int GetColumnHeight(int col)
        {
            for (int row = RowsLength - 1; row > -1; row--)
            {
                if(State(col, row) == FieldState.Free)
                    return row;
            }
            
            return -1;
        }

        public FieldState State(int col, int row)
        {
            if (_boardArray[row][col] == 0) 
                return FieldState.Free;
            if (_boardArray[row][col] == _mybotId) 
                return FieldState.Me;
            return FieldState.Opponent;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < RowsLength; i++)
            {
                for (int j = 0; j < ColsLength; j++)
                {
                    sb.Append(_boardArray[i][j]).Append(" ");
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}