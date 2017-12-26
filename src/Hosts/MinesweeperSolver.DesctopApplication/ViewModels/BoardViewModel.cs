using System;
using System.Windows;
using System.Windows.Controls;
using MinesweeperSolver.GameSimulator;
using MinesweeperSolver.GameSimulator.Models;

namespace MinesweeperSolver.DesctopApplication.ViewModels
{
    public class BoardViewModel : Board
    {
        private readonly Grid _mineField;

        public BoardViewModel(int width, int height, int mineCount, IBoardGeneratorService generator, Grid mineField)
            : base(width, height, mineCount, generator)
        {
            this._mineField = mineField;
        }

        public override void OpenTile(int x, int y)
        {
            base.OpenTile(x, y);

            this.GetTileViewModel(_mineField, x, y)?.Model.NotifyUncovering();
        }

        private MinesweeperTile GetTileViewModel(Grid grid, int column, int row)
        {
            foreach (UIElement child in grid.Children)
            {
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column)
                {
                    return child as MinesweeperTile;
                }
            }

            return null;
        }
    }
}
