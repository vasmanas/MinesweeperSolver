using System;

namespace MinesweeperSolver.GameSimulator.Models
{
    public class DangerTile : Tile
    {
        private byte _nearbyMines;

        public DangerTile(byte nearbyMines) : base(false)
        {
            if (nearbyMines < 1 || nearbyMines > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(nearbyMines), "mines can be between 1 and 8");
            }

            _nearbyMines = nearbyMines;
        }

        public byte NearbyMines => _nearbyMines;
    }
}
