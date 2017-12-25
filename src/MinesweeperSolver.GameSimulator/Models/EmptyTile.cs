using System;

namespace MinesweeperSolver.GameSimulator.Models
{
    public class EmptyTile : Tile
    {
        private readonly Action _openNeighbors;

        public EmptyTile(Action openNeighbors, EndGameTracker endGameTracker) : base(false, endGameTracker)
        {
            _openNeighbors = openNeighbors;
        }

        public override void Open()
        {
            if (!Covered)
            {
                return;
            }

            base.Open();

            _openNeighbors();
        }
    }
}
