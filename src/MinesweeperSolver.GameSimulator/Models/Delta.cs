namespace MinesweeperSolver.GameSimulator.Models
{
    public class Delta
    {
        public Delta(int dx, int dy)
        {
            DeltaX = dx;
            DeltaY = dy;
        }

        public int DeltaX { get; }

        public int DeltaY { get; }
    }
}
