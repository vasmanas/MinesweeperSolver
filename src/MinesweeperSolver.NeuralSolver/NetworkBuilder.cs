using Accord.Neuro;
using System;

namespace MinesweeperSolver.NeuralSolver
{
    public static class NetworkBuilder
    {
        public static ActivationNetwork Create(int lookDistance)
        {
            if (lookDistance < 1)
            {
                throw new ArgumentException("Value mus be greater than 0", nameof(lookDistance));
            }

            /// Inputs:
            /// sqr(2 * lookDistance + 1) + 1
            /// sqr(2 * lookDistance + 1) - board size
            /// 1 - 0 if first run, 1 if second run without any change
            /// 12 :
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

            /// Result:
            /// 1st - if > 0.9 Open tile
            /// 2nd - then if > 0.9 Flag tile

            return new ActivationNetwork(new SigmoidFunction(), (((lookDistance + 1) * lookDistance * 4) + 1) * 12 + 1, 256, 256, 128, 2);
        }
    }
}
