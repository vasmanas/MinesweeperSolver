using MinesweeperSolver.GameSimulator.Models;

namespace MinesweeperSolver.GameSimulator
{
    public interface IBoardGeneratorService
    {
        bool[,] Generate(int width, int height, int mineCount);
    }
}
