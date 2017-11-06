using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolver.GameSimulator.Models
{
    public class Board
    {
        public Board(int width, int height, int bombCount)
        {
            if (width < 1 && width > 100)
            {
                throw new ArgumentException("Value must be between 1 and 100", nameof(width));
            }

            if (height < 1 && height > 100)
            {
                throw new ArgumentException("Value must be between 1 and 100", nameof(height));
            }

            if (bombCount < 0 && bombCount > width * height)
            {
                throw new ArgumentException($"Value must be between 1 and {width * height}", nameof(height));
            }

            Width = width;
            Height = height;
            BombCount = bombCount;
            Squares = new Square[Width, Height];
        }

        public int Width { get; }

        public int Height { get; }

        public int BombCount { get; }

        /// <summary>
        /// Starts at (0,0).
        /// </summary>
        public Square[,] Squares { get; }

        public bool IsBobAt(int x, int y)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
            {
                return false;
            }

            return Squares[x, y] is MineSquare;
        }

        public int SuroundingBombs(int x, int y)
        {
            return (this.IsBobAt(x - 1, y - 1) ? 1 : 0)
            + (this.IsBobAt(x, y - 1) ? 1 : 0)
            + (this.IsBobAt(x + 1, y - 1) ? 1 : 0)

            + (this.IsBobAt(x - 1, y) ? 1 : 0)
            + (this.IsBobAt(x + 1, y) ? 1 : 0)

            + (this.IsBobAt(x - 1, y + 1) ? 1 : 0)
            + (this.IsBobAt(x, y + 1) ? 1 : 0)
            + (this.IsBobAt(x + 1, y + 1) ? 1 : 0);
        }
    }
}
