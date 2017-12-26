namespace MinesweeperSolver.GameSimulator.Models
{
    public abstract class Tile
    {
        private readonly bool _isMine;
        private bool _covered = true;
        private bool _flag = false;

        protected Tile(bool isMine, EndGameTracker endGameTracker)
        {
            _isMine = isMine;
            Tracker = endGameTracker;
        }

        protected EndGameTracker Tracker { get; private set; }
        public bool? Mined => _covered ? (bool?)null : _isMine;
        public bool Covered => _covered;
        public bool Flagged => _flag;

        public virtual void Open()
        {
            if (!_covered)
            {
                return;
            }

            if (_flag)
            {
                Tracker.RemoveFlag(false);

                _flag = false;
            }

            _covered = false;

            Tracker.OpenTile();
        }

        public virtual void Flag()
        {
            if (!_covered)
            {
                return;
            }

            if (_flag)
            {
                Tracker.RemoveFlag(_isMine);

                _flag = false;
            }
            else
            {
                Tracker.PlaceFlag(_isMine);

                _flag = true;
            }
        }
    }
}
