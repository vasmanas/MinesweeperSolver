﻿using MinesweeperSolver.NeuralSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolver.ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var solution = new SolverNet();

            solution.Solve();
        }
    }
}
