using System;

namespace MinesweeperSolver.GameSimulator.Models
{
    public class EndGameTracker
    {
        public EndGameTracker(int totalTileCount, int mineCount)
        {
            this.CoveredTiles = totalTileCount - mineCount;
            this.NotUsedFlags = mineCount;
            this.State = State.Playing;
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
            }
        }

        public void Lost()
        {
            this.State = State.Lost;
        }

        public bool IsFinished()
        {
            return State != State.Playing;
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
    }
}
