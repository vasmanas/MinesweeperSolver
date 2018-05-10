namespace MinesweeperSolver.NeuralSolver
{
    public class PlayStatistics
    {
        public PlayStatistics(
            double score,
            bool won,
            int correctFlags,
            int incorrectFlags,
            int mineCount,
            int openTiles,
            int openableTiles)
        {
            Score = score;
            Won = won;
            CorrectFlags = correctFlags;
            IncorrectFlags = incorrectFlags;
            MineCount = mineCount;
            OpenTiles = openTiles;
            OpenableTiles = openableTiles;
        }

        public double Score { get; }
        public bool Won { get; }
        public int CorrectFlags { get; }
        public int IncorrectFlags { get; }
        public int MineCount { get; }
        public int OpenTiles { get; }
        public int OpenableTiles { get; }
    }
}
