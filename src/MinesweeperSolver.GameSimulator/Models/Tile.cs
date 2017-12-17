namespace MinesweeperSolver.GameSimulator.Models
{
    public class Tile
    {
        private readonly bool _isMine;

        public Tile(bool isMine)
        {
            this._isMine = isMine;
        }

        public bool IsMine => _isMine;
    }
}
