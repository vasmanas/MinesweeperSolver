using MinesweeperSolver.GameSimulator.Models;
using System;
using System.ComponentModel;

namespace MinesweeperSolver.DesctopApplication.ViewModels
{
    public class GameProgress : INotifyPropertyChanged
    {
        private readonly Board _board;
        private readonly Action<string> _reportStatus;

        public GameProgress(Board board, Action<string> reportStatus)
        {
            this._board = board;
            this._reportStatus = reportStatus;
        }

        public State State => _board.EndOfGame;

        public void OpenTile()
        {
            this.NotifyPropertyChanged("State");

            _reportStatus(_board.Statistics);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
