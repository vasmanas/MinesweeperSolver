using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MinesweeperSolver.DesctopApplication.ViewModels
{
    public class TileViewModel : INotifyPropertyChanged
    {
        private bool _covered = true;
        private bool _flag = false;
        private byte _count = 0;
        private bool _mine = false;
        private readonly List<TileViewModel> _neighbours = new List<TileViewModel>();
        private readonly GameProgress _progress;

        public TileViewModel(GameProgress progress)
        {
            _mine = true;
            _progress = progress ?? throw new ArgumentNullException(nameof(progress));
        }

        public TileViewModel(GameProgress progress, byte surroundingBombCount)
        {
            if (surroundingBombCount < 0 || surroundingBombCount > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(surroundingBombCount));
            }

            _count = surroundingBombCount;
            _progress = progress ?? throw new ArgumentNullException(nameof(progress));
        }

        public bool Hidden { get { return _covered && !_flag; } }
        public bool Flagged { get { return _covered && _flag; } }
        public bool Uncovered { get { return !_covered && !_mine; } }
        public string Count { get { return _count == 0 ? string.Empty : _count.ToString(); } }
        public bool Blew { get { return !_covered && _mine; } }

        public void Open()
        {
            if (!this._covered)
            {
                return;
            }

            this._covered = false;

            this.NotifyPropertyChanged("Hidden");
            this.NotifyPropertyChanged("Flagged");
            this.NotifyPropertyChanged("Uncovered");
            this.NotifyPropertyChanged("Blew");

            if (_mine)
            {
                _progress.Lost();

                return;
            }

            _progress.OpenTile();

            if (_count > 0)
            {
                return;
            }

            foreach (var neighbour in _neighbours)
            {
                neighbour.Open();
            }
        }

        public void Flag()
        {
            if (!this._covered)
            {
                return;
            }

            if (_flag)
            {
                _progress.RemoveFlag(_mine);
            }
            else
            {
                _progress.PlaceFlag(_mine);
            }

            this._flag = !this._flag;

            this.NotifyPropertyChanged("Hidden");
            this.NotifyPropertyChanged("Flagged");
        }

        public void Addneighbour(TileViewModel neighbour)
        {
            if (neighbour == null)
            {
                throw new ArgumentNullException(nameof(neighbour));
            }

            if (_mine)
            {
                return;
            }

            this._neighbours.Add(neighbour);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
