using System;
using System.IO;
using System.Linq;
using FourInARow.Strategies;
using System.Diagnostics;

namespace FourInARow
{
    public class Session
    {
        static Stopwatch timer;
        ///<summary>Indicates time bank for current move action.</summary>
        static int TimeBank;

        public Session()
        {
            timer = new Stopwatch();
        }

        public void Run()
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput(512)));
            string line;
            
            Board board = new Board();

            //IStrategy strategy = new Basic();
            IStrategy strategy = new PatternSearchStrategy();

            while ((line = Console.ReadLine()) != null)
            {
                if (line == string.Empty)
                    continue;

                string[] parts = line.Split(' ');

                switch (parts[0])
                {
                    case "settings":
                        switch (parts[1])
                        {
                            case "your_botid":
                                int myBotId = int.Parse(parts[2]);
                                board.Settings.SetMyBotId(myBotId);
                                strategy.UpdateSelfBotId(myBotId);
                                break;
                            case "field_columns":
                                board.SetColumnsLength(int.Parse(parts[2]));
                                strategy.UpdateBoardSize(board);
                                break;
                            case "field_rows":
                                board.SetRowsLength(int.Parse(parts[2]));
                                strategy.UpdateBoardSize(board);
                                break;
                        }
                        break;
                    case "update":
                        switch (parts[1])
                        {
                            case "game":
                                switch (parts[2])
                                {
                                    case "field":
                                        int[][] boardArray = parts[3].Split(';').Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();
                                        board.Update(boardArray);
                                    break;
                                    case "round":
                                        board.Settings.UpdateGameRound(int.Parse(parts[3]));
                                        break;
                                }
                            break;
                        }
                        break;
                    case "action":
                        TimeBank = int.Parse(parts[2]);
                        timer.Restart();
                        int move = strategy.NextMove(board);
                        timer.Stop();
                        if (GlobalVars.PRINT_DEBUG)
                        {
                            Console.WriteLine("Time elapsed: {0} ms", timer.ElapsedMilliseconds);
                        }
                        Console.WriteLine("place_disc {0}", move);  // Output to apply move.
                        break;
                }
            }
        }
    }
}