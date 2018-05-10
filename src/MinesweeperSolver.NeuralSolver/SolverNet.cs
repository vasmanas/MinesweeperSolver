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
using System.Collections.Concurrent;

namespace MinesweeperSolver.NeuralSolver
{
    /// <summary>
    /// http://alishertortay.me/2016/12/22/minesweeper-solver-using-nn/
    /// </summary>
    public class SolverNet
    {
        /// <summary>
        /// Number of tiles to take into account from center tile;
        /// </summary>
        private const int LookDistance = 5;

        public void Solve()
        {
            const int PlayerCount = 100; /// 100;
            const int GenerationCount = 10000; ///10000;
            const int GameCountToPlay = 100; /// 100;
            
            var width = 3;
            var height = 3;
            var mineCount = 1;

            /// Networks
            var scoreBoard = new ScoredNetwork[PlayerCount];
            var bgs = new BoardGeneratorService();

            /// Generations
            for (int gc = 0; gc < GenerationCount; gc++)
            {
                if (gc == 0)
                {
                    for (int i = 0; i < scoreBoard.Length; i++)
                    {
                        /// TODO: weights are positive
                        /// GaussianFunction, BernoulliFunction, SigmoidFunction
                        scoreBoard[i] = new ScoredNetwork(NetworkBuilder.Create(LookDistance));
                    }
                }
                else
                {
                    scoreBoard = RegenerateNetworks(scoreBoard, width, height);
                }

                /// Networks
                foreach (var player in scoreBoard)
                {
                    var scores = new ConcurrentBag<double>();

                    Parallel.For(
                        0,
                        GameCountToPlay,
                        g =>
                        {
                            var board = new Board(width, height, mineCount, bgs);

                            var lost = false;
                            while (board.EndOfGame == State.Playing && !lost)
                            {
                                lost = !IterateBoard(player.Network, width, height, board, true)
                                    && !IterateBoard(player.Network, width, height, board, false);
                            }

                            var score = CaclulateGameScore(board, lost);

                            scores.Add(score);
                        });

                    // Calculate top best games and count them as fitness
                    //player.Fitness = scores.OrderByDescending(e => e).Take(50).Sum();
                    player.Fitness = scores.Sum();
                }
                
                Console.WriteLine($"Top has {scoreBoard.OrderByDescending(e => e.Fitness).First().Fitness}");
            }

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private ScoredNetwork[] RegenerateNetworks(ScoredNetwork[] scores, int width, int height)
        {
            var orderedSocres = scores.OrderByDescending(e => e.Fitness).ToList();

            /// Take to 10 from top
            var top = orderedSocres.Take(10).ToList();
            for (int i = 0; i < top.Count; i++)
            {
                scores[i] = top[i];
            }

            ///// And 9 random
            //var bottom = orderedSocres.Skip(13).ToList();
            //var rnd = new Random();

            //for (int i = 0; i < 9; i++)
            //{
            //    var pos = rnd.Next(bottom.Count);

            //    scores[top.Count + i] = bottom[pos];

            //    bottom.RemoveAt(pos);
            //}

            /// Breed from top only
            /// when total 4
            /// 0:1, 0:2, 0:3
            /// 1:2, 1:3
            /// 2:3
            var startIndex = top.Count/* + 9*/;

            for (int i = 0; i < top.Count - 1; i++)
            {
                for (int j = i + 1; j < top.Count; j++)
                {
                    var children = Breed(top[i].Network, top[j].Network, width, height);

                    scores[startIndex] = new ScoredNetwork(children[0]);
                    scores[startIndex + 1] = new ScoredNetwork(children[1]);

                    startIndex += 2;
                }
            }

            return scores;
        }

        private ActivationNetwork[] Breed(ActivationNetwork mother, ActivationNetwork father, int width, int height)
        {
            var child1 = NetworkBuilder.Create(LookDistance);
            var child2 = NetworkBuilder.Create(LookDistance);
            var rnd = new Random();

            // each layer
            for (int l = 0; l < mother.Layers.Length; l++)
            {
                // each neuron
                for (int n = 0; n < mother.Layers[l].Neurons.Length; n++)
                {
                    ActivationNetwork parent1;
                    ActivationNetwork parent2;

                    if (rnd.Next(2) == 0)
                    {
                        parent1 = mother;
                        parent2 = father;
                    }
                    else
                    {
                        parent1 = father;
                        parent2 = mother;
                    }

                    // each weigth
                    for (int w = 0; w < parent1.Layers[l].Neurons[n].Weights.Length; w++)
                    {
                        child1.Layers[l].Neurons[n].Weights[w] = parent1.Layers[l].Neurons[n].Weights[w];
                        child2.Layers[l].Neurons[n].Weights[w] = parent2.Layers[l].Neurons[n].Weights[w];
                    }
                }
            }

            return new[] { child1, child2 };
        }

        private double CaclulateGameScore(Board board, bool lost)
        {
            double gameScore = 0;
            if (board.EndOfGame == State.Won)
            {
                /// Game won
                gameScore += 10;

                /// Open tiles
                gameScore += 1;

                /// Correct flags
                gameScore += (1.0 * board.CorrectFlags) / board.Mines;
            }
            else
            {
                /// Open tiles
                gameScore += (1.0 * board.OpenedTiles) / (board.Width * board.Height - board.Mines);

                /// Correct flags
                gameScore += (1.0 * board.CorrectFlags) / board.Mines;

                /// TODO: Incorrect flags
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

                    var features = ExtractFeatures(5, x, y, board, firstCycle);

                    /// 1st - Open tile
                    /// 2nd - Flag tile
                    var outputs = network.Compute(features);

                    if (outputs[0] > 0.9)
                    {
                        board.OpenTile(x, y);

                        return true;
                    }
                    else if (outputs[1] > 0.9)
                    {
                        board.Flag(x, y);

                        return true;
                    }
                }
            }

            return false;
        }

        private double[] ExtractFeatures(int range, int x, int y, Board board, bool firstCycle)
        {
            var inputCount = ((range + 1) * range * 4 + 1) * 12 + 1;

            var inputs = new double[inputCount];

            inputs[0] = firstCycle ? 0 : 1;
            
            var minX = x - range;
            var maxX = x + range;
            var minY = y - range;
            var maxY = y + range;
            var sideLength = range * 2 + 1;

            /// (range + 1 + range)x(range + 1 + range)
            for (int dx = minX; dx <= maxX; dx++)
            {
                for (int dy = minY; dy <= maxY; dy++)
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

                    var linePos = 1 + ((dx - minX) * sideLength + (dy - minY)) * 12;

                    if (board.IsOnBoard(dx, dy))
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
}
