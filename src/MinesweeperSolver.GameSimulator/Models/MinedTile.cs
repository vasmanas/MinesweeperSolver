using System;

namespace MinesweeperSolver.GameSimulator.Models
{
    public class MinedTile : Tile
    {
        public MinedTile(EndGameTracker endGameTracker) : base(true, endGameTracker)
        {
        }

        public override void Open()
        {
            Tracker.Lost();
        }
    }
}
