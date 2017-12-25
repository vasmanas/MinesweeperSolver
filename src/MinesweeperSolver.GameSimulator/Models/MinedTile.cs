﻿using System;

namespace MinesweeperSolver.GameSimulator.Models
{
    public class MinedTile : Tile
    {
        private readonly Action _blow;

        public MinedTile(Action blow, EndGameTracker endGameTracker) : base(true, endGameTracker)
        {
            this._blow = blow;
        }

        public override void Open()
        {
            _blow();
        }
    }
}
