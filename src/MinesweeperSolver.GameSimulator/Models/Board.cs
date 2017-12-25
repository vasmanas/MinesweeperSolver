﻿using System;
using System.Collections.Generic;

namespace MinesweeperSolver.GameSimulator.Models
{
    public class Board
    {
        private static readonly IReadOnlyList<Delta> Deltas
            = new List<Delta>
            {
                new Delta(-1, -1),
                new Delta(0, -1),
                new Delta(1, -1),
                new Delta(-1, 0),
                new Delta(1, 0),
                new Delta(-1, 1),
                new Delta(0, 1),
                new Delta(1, 1)
            }.AsReadOnly();

        /// <summary>
        /// Starts at (0,0).
        /// </summary>
        private readonly Tile[,] _tiles;
        private readonly Action<int, int> _postOpen;
        private readonly EndGameTracker _endGameTracker;

        public Board(
            int width,
            int height,
            int mineCount,
            IBoardGeneratorService generator,
            Action blow,
            Action<int, int> postOpen,
            Action<string> wonGameAction)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            if (width < 1 && width > 100)
            {
                throw new ArgumentException("Value must be between 1 and 100", nameof(width));
            }

            if (height < 1 && height > 100)
            {
                throw new ArgumentException("Value must be between 1 and 100", nameof(height));
            }

            if (mineCount < 1 && mineCount > width * height)
            {
                throw new ArgumentOutOfRangeException(nameof(mineCount), "Mine count myt be between 1 and width * height");
            }

            this._postOpen = postOpen;
            Width = width;
            Height = height;

            _endGameTracker = new EndGameTracker(width * height, mineCount, wonGameAction);

            _tiles = new Tile[Width, Height];
            var mines = generator.Generate(Width, Height, mineCount);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (mines[i, j])
                    {
                        _tiles[i, j] = new MinedTile(() => { _endGameTracker.Lost(); blow(); }, _endGameTracker);
                    }
                    else
                    {
                        var cnt = SuroundingMines(i, j, mines);

                        if (cnt > 0)
                        {
                            _tiles[i, j] = new DangerTile((byte)cnt, _endGameTracker);
                        }
                        else
                        {
                            var cx = i;
                            var cy = j;

                            _tiles[i, j] = new EmptyTile(() => this.IterateNeighbors(cx, cy, (x, y) => this.OpenTile(x, y)), _endGameTracker);
                        }
                    }
                }
            }
        }

        public int Width { get; }

        public int Height { get; }

        public State EndOfGame => _endGameTracker.State;

        public bool IsInBoard(int x, int y)
        {
            return !(x < 0 || x > Width - 1 || y < 0 || y > Height - 1);
        }

        public void Flag(int x, int y)
        {
            if (!IsInBoard(x, y))
            {
                return;
            }

            _tiles[x, y].Flag();
        }

        public bool IsFlagged(int x, int y)
        {
            if (!IsInBoard(x, y))
            {
                return false;
            }

            return _tiles[x, y].Flagged;
        }

        public void OpenTile(int x, int y)
        {
            if (!IsInBoard(x, y))
            {
                return;
            }

            _tiles[x, y].Open();

            _postOpen(x, y);
        }

        public bool IsCovered(int x, int y)
        {
            if (!IsInBoard(x, y))
            {
                return false;
            }

            return _tiles[x, y].Covered;
        }

        public bool? IsMine(int x, int y)
        {
            if (!IsInBoard(x, y))
            {
                return null;
            }

            if (IsCovered(x, y))
            {
                return null;
            }

            return _tiles[x, y].Mined;
        }

        /// <summary>
        /// 0-8, -1 if is covered.
        /// </summary>
        public int? SurroundingMineCount(int x, int y)
        {
            if (!IsInBoard(x, y))
            {
                return null;
            }

            if (IsCovered(x, y))
            {
                return null;
            }

            if (_tiles[x, y] is DangerTile)
            {
                return (_tiles[x, y] as DangerTile).NearbyMines;
            }

            if (_tiles[x, y] is EmptyTile)
            {
                return 0;
            }

            return null;
        }

        private int SuroundingMines(int x, int y, bool[,] tiles)
        {
            var count = 0;

            IterateNeighbors(x, y, (dx, dy) => count += tiles[dx, dy] ? 1 : 0);

            return count;
        }

        private List<Tile> GetNeighbors(int x, int y)
        {
            var result = new List<Tile>();

            IterateNeighbors(x, y, (dx, dy) => result.Add(_tiles[dx, dy]));

            return result;
        }

        private void IterateNeighbors(int x, int y, Action<int, int> action)
        {
            foreach (var delta in Deltas)
            {
                if (IsInBoard(x + delta.DeltaX, y + delta.DeltaY))
                {
                    action(x + delta.DeltaX, y + delta.DeltaY);
                }
            }
        }
    }
}
