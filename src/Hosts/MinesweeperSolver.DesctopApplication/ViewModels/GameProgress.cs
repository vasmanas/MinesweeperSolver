using MinesweeperSolver.GameSimulator.Models;
using System;
using System.ComponentModel;

namespace MinesweeperSolver.DesctopApplication.ViewModels
{
    public class GameProgress : INotifyPropertyChanged
    {
        private readonly Board _board;

        public GameProgress(Board board)
        {
            this._board = board;
        }

        public State State => _board.EndOfGame;

        public void OpenTile()
        {
            this.NotifyPropertyChanged("State");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string Statistics()
        {
            return $"{State}";
            //return $"{State};CF:{CorrectFlags};IF:{IncorrectFlags};NF:{NotUsedFlags};OT:{OpenTiles};CT:{CoveredTiles}";
        }
    }
}
