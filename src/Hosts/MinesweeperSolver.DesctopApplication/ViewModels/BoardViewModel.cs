using System;
using MinesweeperSolver.GameSimulator;
using MinesweeperSolver.GameSimulator.Models;

namespace MinesweeperSolver.DesctopApplication.ViewModels
{
    public class BoardViewModel : Board
    {
        private readonly Action<int, int> _postOpen;

        public BoardViewModel(int width, int height, int mineCount, IBoardGeneratorService generator, Action<int, int> postOpen)
            : base(width, height, mineCount, generator)
        {
            this._postOpen = postOpen;
        }

        public override void OpenTile(int x, int y)
        {
            if (!IsInBoard(x, y))
            {
                return;
            }

            base.OpenTile(x, y);

            _postOpen(x, y);
        }
    }
}
