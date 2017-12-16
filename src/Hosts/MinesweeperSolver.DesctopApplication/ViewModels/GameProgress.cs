using System;
using System.ComponentModel;

namespace MinesweeperSolver.DesctopApplication.ViewModels
{
    public class GameProgress : INotifyPropertyChanged
    {
        private readonly Action<string> _endGame;

        public GameProgress(int totalTileCount, int mineCount, Action<string> endGame)
        {
            this.CoveredTiles = totalTileCount - mineCount;
            this.NotUsedFlags = mineCount;
            this.State = State.Playing;
            _endGame = endGame;
        }

        public State State { get; private set; }

        public int OpenTiles { get; private set; }
        public int CoveredTiles { get; private set; }

        public int CorrectFlags { get; private set; }
        public int IncorrectFlags { get; private set; }
        public int NotUsedFlags { get; private set; }

        public void OpenTile()
        {
            this.OpenTiles++;
            this.CoveredTiles--;

            if (this.CoveredTiles == 0)
            {
                this.State = State.Won;

                this.NotifyPropertyChanged("State");

                _endGame(Statistics());
            }
        }

        public void Lost()
        {
            this.State = State.Lost;

            this.NotifyPropertyChanged("State");

            _endGame(Statistics());
        }

        public void PlaceFlag(bool correct)
        {
            this.NotUsedFlags--;

            if (correct)
            {
                this.CorrectFlags++;
            }
            else
            {
                this.IncorrectFlags++;
            }
        }

        public void RemoveFlag(bool correct)
        {
            this.NotUsedFlags++;

            if (correct)
            {
                this.CorrectFlags--;
            }
            else
            {
                this.IncorrectFlags--;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string Statistics()
        {
            return $"{State};CF:{CorrectFlags};IF:{IncorrectFlags};NF:{NotUsedFlags};OT:{OpenTiles};CT:{CoveredTiles}";
        }
    }
}
