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
        private bool _bomb = false;
        private readonly List<TileViewModel> _neighbours = new List<TileViewModel>();

        public TileViewModel()
        {
            _bomb = true;
        }

        public TileViewModel(byte surroundingBombCount)
        {
            if (surroundingBombCount < 0 || surroundingBombCount > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(surroundingBombCount));
            }

            _count = surroundingBombCount;
        }

        public bool Hidden { get { return _covered && !_flag; } }
        public bool Flagged { get { return _covered && _flag; } }
        public bool Uncovered { get { return !_covered && !_bomb; } }
        public string Count {
            get
            {
                if (_count > 0)
                {
                    return _count.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public bool Blew
        {
            get
            {
                return !_covered && _bomb;
            }
        }

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

            if (_bomb)
            {
                return;
            }

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

            if (_bomb)
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
