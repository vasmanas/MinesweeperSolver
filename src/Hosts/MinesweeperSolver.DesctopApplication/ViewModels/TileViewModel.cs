using MinesweeperSolver.GameSimulator.Models;
using System.ComponentModel;

namespace MinesweeperSolver.DesctopApplication.ViewModels
{
    public class TileViewModel : INotifyPropertyChanged
    {
        private readonly Board _borad;
        private readonly int _x;
        private readonly int _y;
        private readonly GameProgress _progress;

        public TileViewModel(Board borad, int x, int y, GameProgress progress)
        {
            this._borad = borad;
            this._x = x;
            this._y = y;
            this._progress = progress;
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

            _borad.OpenTile(_x, _y);

            NotifyUncovering();

            _progress.OpenTile();
        }

        public void Flag()
        {
            if (!_borad.IsCovered(_x, _y))
            {
                return;
            }

            _borad.Flag(_x, _y);

            NotifyFlagging();
        }

        public void NotifyUncovering()
        {
            NotifyFlagging();

            this.NotifyPropertyChanged("Uncovered");
            this.NotifyPropertyChanged("Count");
            this.NotifyPropertyChanged("Blew");
        }

        private void NotifyFlagging()
        {
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
