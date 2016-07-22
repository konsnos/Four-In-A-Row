using System.Diagnostics;

namespace FourInARow.Strategies
{
    public interface IStrategy
    {
        int NextMove(Board board);
        void UpdateSelfBotId(int newMyBotId);
        void UpdateBoardSize(Board board);
    }
}
