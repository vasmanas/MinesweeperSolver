namespace MinesweeperSolver.GameSimulator.Models
{
    public abstract class Tile
    {
        private readonly bool _isMine;
        private readonly EndGameTracker _endGameTracker;
        private bool _covered = true;
        private bool _flag = false;

        protected Tile(bool isMine, EndGameTracker endGameTracker)
        {
            _isMine = isMine;
            _endGameTracker = endGameTracker;
        }

        public bool Mined => _isMine;
        public bool Covered => _covered;
        public bool Flagged => _flag;

        public virtual void Open()
        {
            if (!_covered)
            {
                return;
            }

            _covered = false;
            _flag = false;

            _endGameTracker.OpenTile();
        }

        public virtual void Flag()
        {
            if (!_covered)
            {
                return;
            }

            _flag = !_flag;
        }
    }
}
