using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperSolver.GameSimulator.Models;

namespace MinesweeperSolver.GameSimulator.Tests
{
    [TestClass]
    public class MineGeneratorTests
    {
        private const string Category = "MineGenerator";

        [TestMethod]
        [TestCategory(Category)]
        public void Generate_Single_Mine()
        {
            var board = new Board(1, 1);
            var generator = new MineGenerator();

            Assert.IsNull(board.Tiles[0, 0]);

            generator.Fill(board, 1);

            Assert.IsTrue(board.Tiles[0, 0].IsMine);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Generate_Multiple_Mines()
        {
            const int MineCount = 10;

            var board = new Board(9, 9);
            var generator = new MineGenerator();

            generator.Fill(board, MineCount);

            var emptyTileCount = 0;
            var minedTileCount = 0;
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    if (board.Tiles[i, j].IsMine)
                    {
                        minedTileCount++;
                    }
                    else
                    {
                        emptyTileCount++;
                    }
                }
            }

            Assert.AreEqual(MineCount, minedTileCount);
            Assert.AreEqual((board.Width * board.Height) - MineCount, emptyTileCount);
        }
    }
}