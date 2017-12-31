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

            var width = 10;
            var height = 10;
            var mineCount = 10;

            /// Generations
            for (int gc = 0; gc < 1000; gc++)
            {
                /// Networks
                foreach (var player in scoreBoard)
                {
                    var scores = new ConcurrentBag<double>();

                    Parallel.For(
                        0,
                        100,
                        g =>
                        {
                            var board = new Board(width, height, mineCount, new BoardGeneratorService());

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
                    player.Fitness = scores.OrderByDescending(e => e).Take(50).Sum();
                }

                scoreBoard = RegenerateNetworks(scoreBoard);
            }

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private ScoredNetwork[] RegenerateNetworks(ScoredNetwork[] scores)
        {
            var orderedSocres = scores.OrderByDescending(e => e.Fitness).ToList();

            Console.WriteLine($"Top has {orderedSocres.First().Fitness}");

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
                    var children = Breed(top[i].Network, top[j].Network);

                    scores[startIndex] = new ScoredNetwork(children[0]);
                    scores[startIndex + 1] = new ScoredNetwork(children[1]);

                    startIndex += 2;
                }
            }

            return scores;
        }

        private ActivationNetwork[] Breed(ActivationNetwork mother, ActivationNetwork father)
        {
            var child1 = new DeepBeliefNetwork(new GaussianFunction(), 1 + (11 * 11 * 12), 256, 256, 128, 2);
            var child2 = new DeepBeliefNetwork(new GaussianFunction(), 1 + (11 * 11 * 12), 256, 256, 128, 2);
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
}
