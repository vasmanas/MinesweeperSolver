using MinesweeperSolver.GameSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolver.GameSimulator
{
    public class MineGenerator
    {
        public void Fill(Board board, int mineCount)
        {
            if (mineCount < 0 || board.Width * board.Height < mineCount)
            {
                throw new ArgumentException("Value must be more than 0 and less than wigth * height", nameof(mineCount));
            }

            var rnd = new Random();

            for (int i = 0; i < mineCount; i++)
            {
                int x;
                int y;

                do
                {
                    x = rnd.Next(0, board.Width);
                    y = rnd.Next(0, board.Height);
                } while (board.IsMineAt(x, y));


                board.Tiles[x, y] = new MineTile();
            }

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    if (board.Tiles[i, j] is null)
                    {
                        board.Tiles[i, j] = new EmptyTile();
                    }
                }
            }
        }
    }
}
