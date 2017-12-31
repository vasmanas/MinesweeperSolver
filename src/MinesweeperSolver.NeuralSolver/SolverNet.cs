using Accord.Neuro;
using Accord.Neuro.Networks;
using Accord.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Neuro.ActivationFunctions;
using MinesweeperSolver.GameSimulator.Models;
using MinesweeperSolver.GameSimulator;

namespace MinesweeperSolver.NeuralSolver
{
    /// <summary>
    /// http://alishertortay.me/2016/12/22/minesweeper-solver-using-nn/
    /// </summary>
    public class SolverNet
    {
        public void Solve()
        {
            /// 1: Off limits
            /// 1: Covered
            /// 1: Flag
            /// 9: 0-8
            /// Total: 12
            /// 11x11x12=1452

            /// Networks
            ScoredNetwork[] scoreBoard = new ScoredNetwork[100];
            for (int i = 0; i < scoreBoard.Length; i++)
            {
                /// BernoulliFunction, SigmoidFunction
                scoreBoard[i] = new ScoredNetwork(new DeepBeliefNetwork(new GaussianFunction(), 1 + (11 * 11 * 12), 256, 256, 128, 2));
            }

            //var teacher = new EvolutionaryLearning(network, 100);
            //var teacher = new BackPropagationLearning(network)
            //{
            //    LearningRate = 0.1,
            //    Momentum = 0.9
            //};

            var width = 10;
            var height = 10;

            /// Generations
            for (int gc = 0; gc < 40; gc++)
            {
                /// Networks
                foreach (var player in scoreBoard)
                {
                    player.Fitness = 0;

                    /// Game count
                    for (int g = 0; g < 100; g++)
                    {
                        var board = new Board(width, height, 10, new BoardGeneratorService());

                        var lost = false;
                        while (board.EndOfGame == State.Playing && !lost)
                        {
                            lost = !IterateBoard(player.Network, width, height, board, true)
                                && !IterateBoard(player.Network, width, height, board, false);
                        }

                        // TODO: Calculate top 10 best games and count them as fitness

                        player.Fitness += CaclulateGameScore(board, lost);
                    }
                }

                scoreBoard = RegenerateNetworks(scoreBoard);
            }
        }

        private ScoredNetwork[] RegenerateNetworks(ScoredNetwork[] scores)
        {
            var orderedSocres = scores.OrderByDescending(e => e.Fitness).ToList();

            Console.WriteLine($"Top has {orderedSocres.First().Fitness}");

            /// Take to 13 from top
            var top = orderedSocres.Take(13).ToList();
            for (int i = 0; i < top.Count; i++)
            {
                scores[i] = top[i];
            }

            /// And 9 random
            var bottom = orderedSocres.Skip(13).ToList();
            var rnd = new Random();

            for (int i = 0; i < 9; i++)
            {
                var pos = rnd.Next(bottom.Count);

                scores[top.Count + i] = bottom[pos];

                bottom.RemoveAt(pos);
            }

            /// Breed from top only
            /// when total 4
            /// 0:1, 0:2, 0:3
            /// 1:2, 1:3
            /// 2:3
            var startIndex = top.Count + 9;

            for (int i = 0; i < 12; i++)
            {
                for (int j = i + 1; j < 13; j++)
                {
                    var child = Breed(top[i].Network, top[j].Network);

                    scores[startIndex] = new ScoredNetwork(child);

                    startIndex++;
                }
            }

            return scores;
        }

        private ActivationNetwork Breed(ActivationNetwork mother, ActivationNetwork father)
        {
            var child = new DeepBeliefNetwork(new GaussianFunction(), 1 + (11 * 11 * 12), 256, 256, 128, 2);
            var rnd = new Random();

            // each layer
            for (int l = 0; l < mother.Layers.Length; l++)
            {
                // each neuron
                for (int n = 0; n < mother.Layers[l].Neurons.Length; n++)
                {
                    // each weigth
                    for (int w = 0; w < mother.Layers[l].Neurons[n].Weights.Length; w++)
                    {
                        child.Layers[l].Neurons[n].Weights[w] =
                            rnd.Next(2) == 0 ? mother.Layers[l].Neurons[n].Weights[w] : father.Layers[l].Neurons[n].Weights[w];
                    }
                }
            }

            return child;
        }

        private double CaclulateGameScore(Board board, bool lost)
        {
            double gameScore = 0;
            if (lost)
            {
                /// Open tiles
                gameScore += (1.0 * board.OpenedTiles) / (board.Width * board.Height - board.Mines);
            }
            else
            {
                if (board.EndOfGame == State.Won)
                {
                    /// Open tiles
                    gameScore += 1;

                    /// Correct flags
                    gameScore += 1;
                }
                else
                {
                    /// Open tiles
                    gameScore += (1.0 * board.OpenedTiles) / (board.Width * board.Height - board.Mines);

                    /// Correct flags
                    gameScore += ((1.0 * board.CorrectFlags) / board.Mines);

                    /// TODO: Incorrect flags
                }
            }

            return gameScore;
        }

        private bool IterateBoard(ActivationNetwork network, int width, int height, Board board, bool firstCycle)
        {
            /// x 
            for (int x = 0; x < width; x++)
            {
                /// y
                for (int y = 0; y < height; y++)
                {
                    if (!board.IsCovered(x, y))
                    {
                        continue;
                    }

                    var features = ExtractFeatures(board, 5, x, y, firstCycle);

                    /// 1st - Open
                    /// 2nd - Flag
                    var outputs = network.Compute(features);

                    if (outputs[0] > 0.5)
                    {
                        board.OpenTile(x, y);

                        return true;
                    }
                    else if (outputs[1] > 0.5)
                    {
                        board.Flag(x, y);

                        return true;
                    }
                }
            }

            return false;
        }

        private double[] ExtractFeatures(Board board, int range, int x, int y, bool firstCycle)
        {
            var inputCount = (range * 2 + 1) * (range * 2 + 1) * 12 + 1;

            var inputs = new double[inputCount];

            inputs[0] = firstCycle ? 0 : 1;

            /// (range + 1 + range)x(range + 1 + range)
            for (int dx = x - range; dx <= x + range; dx++)
            {
                for (int dy = y - range; dy <= y + range; dy++)
                {
                    ///  0 - {off limits}
                    ///  1 - {Covered}
                    ///  2 - {Flagged}
                    ///  3 - {0}
                    ///  4 - {1}
                    ///  5 - {2}
                    ///  6 - {3}
                    ///  7 - {4}
                    ///  8 - {5}
                    ///  9 - {6}
                    /// 10 - {7}
                    /// 11 - {8}

                    var linePos = 1 + ((dx - (x - range)) * (range * 2 + 1) + (dy - (y - range))) * 12;

                    if (board.IsInBoard(dx, dy))
                    {
                        if (board.IsCovered(dx, dy))
                        {
                            if (board.IsFlagged(dx, dy))
                            {
                                linePos += 2;
                            }
                            else
                            {
                                linePos += 1;
                            }
                        }
                        else
                        {
                            var mines = board.SurroundingMineCount(dx, dy);

                            linePos += 3 + mines.Value;
                        }
                    }

                    inputs[linePos] = 1;
                }
            }

            return inputs;
        }
    }


    ////private void Train(Network netModel, Dispatcher dispatcher)
    ////    {
    ////            var localLowestError = double.MaxValue;

    ////    /// Networks
    ////    ActivationNetwork network;

    ////    //network = new DeepBeliefNetwork(new BernoulliFunction(), 1024, 2048, 62);
    ////    //network = new DeepBeliefNetwork(new GaussianFunction(), 1024, 744, 62);
    ////    //network = new DeepBeliefNetwork(new BernoulliFunction(), 1024, 62);
    ////    //network = new ActivationNetwork(new SigmoidFunction(), 1024, 62);

    ////    //var nn = MakeName(network);
    ////    //network.Save(string.Format("{0}\\{1}_{2:yyyyMMdd_HHmmssfff}.ann", nn, localLowestError, DateTime.UtcNow));

    ////    /// Teachers
    ////    //DeepBeliefNetworkLearning teacher = new DeepBeliefNetworkLearning(network)
    ////    //{
    ////    //    Algorithm = (h, v, i) => new ContrastiveDivergenceLearning(h, v)
    ////    //    {
    ////    //        LearningRate = 0.1,
    ////    //        Momentum = 0.5,
    ////    //        Decay = 0.001,
    ////    //    },
    ////    //    LayerIndex = SelectedLayerIndex - 1,
    ////    //};

    ////    //network = DeepBeliefNetwork.Load("C:\\work\\1024_62_20170307_1543.ann");
    ////    var filePath = GetBestSave(netModel.FullPath, out localLowestError);
    ////    network = DeepBeliefNetwork.Load(filePath);

    ////            dispatcher.BeginInvoke((Action<Network, int, double>) UpdateError, DispatcherPriority.ContextIdle, netModel, 0, localLowestError);

    ////            var teacher = new BackPropagationLearning(network)
    ////            {
    ////                LearningRate = 0.1,
    ////                Momentum = 0.9
    ////            };

    ////    //DeepNeuralNetworkLearning teacher = new DeepNeuralNetworkLearning(Main.Network)
    ////    //{
    ////    //    Algorithm = (ann, i) => new ParallelResilientBackpropagationLearning(ann),
    ////    //    LayerIndex = Main.Network.Layers.Length - 1,
    ////    //};

    ////    double[][] inputs, outputs;

    ////    inputs = new double[this.OriginalLetters.Count][];
    ////            outputs = new double[this.OriginalLetters.Count][];

    ////            for (int i = 0; i<inputs.Length; i++)
    ////            {
    ////                //int total = set[i].Database.Classes;

    ////                inputs[i] = this.OriginalLetters[i].Features;
    ////                outputs[i] = new double[62];

    ////                var charCode = (byte)this.OriginalLetters[i].Character;

    ////                if (charCode >= 48 && charCode <= 57)
    ////                {
    ////        // 0..9: 48 -  57 (52 - 61)
    ////        outputs[i][charCode - 48 + 52] = 1;
    ////    }
    ////                else if (charCode >= 65 && charCode <= 90)
    ////                {
    ////        // A..Z: 65 -  90 (26 - 51)
    ////        outputs[i][charCode - 65 + 26] = 1;
    ////    }
    ////                else if (charCode >= 97 && charCode <= 122)
    ////                {
    ////        // a..z: 97 - 122 ( 0 - 25)
    ////        outputs[i][charCode - 97] = 1;
    ////    }
    ////    }

    ////    Utils.Normalize(inputs);

    ////            var cnt = 0;
    ////    var lastOutput = DateTime.UtcNow;
    ////    var networkName = MakeName(network);

    ////            while (true)
    ////            {
    ////                Utils.Randomize(ref inputs, ref outputs);

    ////                // run epoch of learning procedure
    ////                double error = teacher.RunEpoch(inputs, outputs);

    ////                // check error value to see
    ////                if (error< 0.45)
    ////                {
    ////                    System.Diagnostics.Debug.WriteLine("learned at {0}", cnt);

    ////                    break;
    ////                }

    ////                if (network is DeepBeliefNetwork)
    ////                {
    ////                    ((DeepBeliefNetwork) network).UpdateVisibleWeights();
    ////                }

    ////                if ((DateTime.UtcNow - lastOutput).TotalSeconds > 5)
    ////                {
    ////                    lastOutput = DateTime.UtcNow;

    ////                    System.Diagnostics.Debug.WriteLine("{2:yyyy-MM-dd HH:mm:ss.fff} {0}. error: {1}", cnt, error, lastOutput);

    ////                    dispatcher.BeginInvoke((Action<Network, int, double>) UpdateError, DispatcherPriority.ContextIdle, netModel, cnt + 1, error);
    ////                }

    ////                if (localLowestError > error)
    ////                {
    ////                    if (!Directory.Exists(networkName))
    ////                    {
    ////                        Directory.CreateDirectory(networkName);
    ////                    }

    ////                    network.Save(string.Format("{0}\\{1}_{2:yyyyMMdd_HHmmssfff}.ann", networkName, error, DateTime.UtcNow));

    ////                    localLowestError = error;

    ////                    dispatcher.BeginInvoke((Action<Network, int, double>) UpdateError, DispatcherPriority.ContextIdle, netModel, cnt + 1, error);
    ////                }

    ////                cnt++;
    ////            }
    ////        }

    ////        public void StopTraining(Network network)
    ////{
    ////    var source = network.TokenSource;

    ////    network.TrainingTask = null;
    ////    network.TokenSource = null;

    ////    source.Cancel();
    ////}

    ////private void UpdateError(Network network, int epoch, double error)
    ////{
    ////    network.CurrentEpoch = epoch;
    ////    network.CurrentError = error;
    ////}

    ////public static string GetBestSave(string networkSavePath, out double minError)
    ////{
    ////    var fileNames = Directory.EnumerateFiles(networkSavePath, "*.ann");

    ////    minError = double.MaxValue;
    ////    var minFileName = string.Empty;

    ////    foreach (var fileName in fileNames)
    ////    {
    ////        var fileNameParts = Path.GetFileName(fileName).Split('_');

    ////        if (double.TryParse(fileNameParts[0].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double errorValue))
    ////        {
    ////            if (errorValue < minError)
    ////            {
    ////                minError = errorValue;
    ////                minFileName = fileName;
    ////            }
    ////        }
    ////    }

    ////    return minFileName;
    ////}

    ////private static string MakeName(ActivationNetwork network)
    ////{
    ////    var layers = network.Layers;
    ////    var name = string.Empty;
    ////    for (int i = 0; i < layers.Length; i++)
    ////    {
    ////        name += string.Format("{0}_", layers[i].InputsCount);

    ////        if (i == layers.Length - 1)
    ////        {
    ////            name += string.Format("{0}", layers[i].Neurons.Length);
    ////        }
    ////    }

    ////    return name;
    ////}

    ////private static double[] ToFeatures(Bitmap bmp)
    ////{
    ////    var features = new double[32 * 32];

    ////    for (int i = 32 * 32 - 1; i >= 0; i--)
    ////    {
    ////        features[i] = 255;
    ////    }

    ////    for (int i = 0; i < bmp.Height; i++)
    ////        for (int j = 0; j < bmp.Width; j++)
    ////            features[i * 32 + j] = (bmp.GetPixel(j, i).R > 0) ? 0 : 1;

    ////    return features;
    ////}
}
