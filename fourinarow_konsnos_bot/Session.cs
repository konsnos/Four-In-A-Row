using System;
using System.IO;
using System.Linq;
using FourInARow.Strategies;

namespace FourInARow
{
    public class Session
    {
        
        public void Run()
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput(512)));
            string line;
            
            Board board = new Board();
            IStrategy strategy = new Basic();

            while ((line = Console.ReadLine()) != null)
            {
                if (line == string.Empty) continue;

                string[] parts = line.Split(' ');

                switch (parts[0])
                {
                    case "settings":
                        switch (parts[1])
                        {
                            case "your_botid":
                                int myBotId = int.Parse(parts[2]);
                                board.SetMyBotId(myBotId);
                                break;
                            case "field_columns":
                                board.SetColumnsLength(int.Parse(parts[2]));
                                break;
                            case "field_rows":
                                board.SetRowsLength(int.Parse(parts[2]));
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
                                }
                            break;
                        }
                        break;
                    case "action":
                        int move = strategy.NextMove(board);
                        Console.WriteLine("place_disc {0}", move);  // Output to apply move.
                        break;
                }
            }
        }
    }
}