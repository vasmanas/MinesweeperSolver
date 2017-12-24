using System;

namespace MinesweeperSolver.GameSimulator.Models
{
    public class EmptyTile : Tile
    {
        private readonly Action _openNeighbors;

        public EmptyTile(Action openNeighbors) : base(false)
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
