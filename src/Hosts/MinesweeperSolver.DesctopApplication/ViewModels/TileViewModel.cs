using MinesweeperSolver.GameSimulator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MinesweeperSolver.DesctopApplication.ViewModels
{
    public class TileViewModel : INotifyPropertyChanged
    {
        private readonly Board _borad;
        private readonly int _x;
        private readonly int _y;

        public TileViewModel(Board borad, int x, int y)
        {
            this._borad = borad;
            this._x = x;
            this._y = y;

            //_borad.IsCovered(x, y);
            //_borad.IsFlagged(x, y);
        }

        public bool Hidden { get { return _borad.IsCovered(_x, _y) && !_borad.IsFlagged(_x, _y); } }
        public bool Flagged { get { return _borad.IsCovered(_x, _y) && _borad.IsFlagged(_x, _y); } }
        public bool Uncovered { get { return !_borad.IsCovered(_x, _y) && _borad.IsMine(_x, _y).HasValue && !_borad.IsMine(_x, _y).Value; } }
        public bool Blew { get { return !_borad.IsCovered(_x, _y) && _borad.IsMine(_x, _y).HasValue && _borad.IsMine(_x, _y).Value; } }
        public string Count { get { return (_borad.SurroundingMineCount(_x, _y) ?? 0) == 0 ? string.Empty : _borad.SurroundingMineCount(_x, _y).Value.ToString(); } }

        public void Open()
        {
            if (!_borad.IsCovered(_x, _y))
            {
                return;
            }

            // TOOD: jei atsidare tuscias tai atidaryti ir kaimynus
            _borad.OpenTile(_x, _y);

            this.NotifyPropertyChanged("Hidden");
            this.NotifyPropertyChanged("Flagged");
            this.NotifyPropertyChanged("Uncovered");
            this.NotifyPropertyChanged("Blew");
            this.NotifyPropertyChanged("Count");
        }

        public void Flag()
        {
            if (!_borad.IsCovered(_x, _y))
            {
                return;
            }

            _borad.Flag(_x, _y);

            this.NotifyPropertyChanged("Hidden");
            this.NotifyPropertyChanged("Flagged");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
