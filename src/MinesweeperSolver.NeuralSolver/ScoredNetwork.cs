using Accord.Neuro;

namespace MinesweeperSolver.NeuralSolver
{
    public class ScoredNetwork
    {
        public ScoredNetwork(ActivationNetwork network)
        {
            Network = network;
        }

        public double Fitness { get; set; }

        public ActivationNetwork Network { get; }
    }
}
