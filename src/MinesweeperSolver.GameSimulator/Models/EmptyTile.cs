namespace MinesweeperSolver.GameSimulator.Models
{
    public class EmptyTile : Tile
    {
        public override bool IsMine()
        {
            return false;
        }
    }
}
