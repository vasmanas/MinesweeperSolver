namespace MinesweeperSolver.GameSimulator.Models
{
    public class MineTile : Tile
    {
        public override bool IsMine()
        {
            return true;
        }
    }
}
