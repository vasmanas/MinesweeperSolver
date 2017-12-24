using System;

namespace MinesweeperSolver.GameSimulator
{
    public class BoardGeneratorService : IBoardGeneratorService
    {
        public bool[,] Generate(int width, int height, int mineCount)
        {
            if (mineCount < 0 || width * height < mineCount)
            {
                throw new ArgumentException("Value must be more than 0 and less than wigth * height", nameof(mineCount));
            }

            var tiles = new bool[width, height];
            var rnd = new Random();

            for (int i = 0; i < mineCount; i++)
            {
                int x;
                int y;

                do
                {
                    x = rnd.Next(0, width - 1);
                    y = rnd.Next(0, height - 1);
                } while (tiles[x, y]);

                tiles[x, y] = true;
            }

            return tiles;
        }
    }
}
