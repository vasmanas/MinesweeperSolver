using System;

namespace MinesweeperSolver.GameSimulator.Models
{
    public class Board
    {
        public Board(int width, int height)
        {
            if (width < 1 && width > 100)
            {
                throw new ArgumentException("Value must be between 1 and 100", nameof(width));
            }

            if (height < 1 && height > 100)
            {
                throw new ArgumentException("Value must be between 1 and 100", nameof(height));
            }

            Width = width;
            Height = height;
            Tiles = new Tile[Width, Height];
        }

        public int Width { get; }

        public int Height { get; }

        /// <summary>
        /// Starts at (0,0).
        /// </summary>
        public Tile[,] Tiles { get; }

        public virtual bool IsMineAt(int x, int y)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
            {
                return false;
            }

            if (Tiles[x, y] is null)
            {
                return false;
            }

            return Tiles[x, y].IsMine;
        }

        public virtual int SuroundingMines(int x, int y)
        {
            return (this.IsMineAt(x - 1, y - 1) ? 1 : 0)
            + (this.IsMineAt(x, y - 1) ? 1 : 0)
            + (this.IsMineAt(x + 1, y - 1) ? 1 : 0)

            + (this.IsMineAt(x - 1, y) ? 1 : 0)
            + (this.IsMineAt(x + 1, y) ? 1 : 0)

            + (this.IsMineAt(x - 1, y + 1) ? 1 : 0)
            + (this.IsMineAt(x, y + 1) ? 1 : 0)
            + (this.IsMineAt(x + 1, y + 1) ? 1 : 0);
        }
    }
}
