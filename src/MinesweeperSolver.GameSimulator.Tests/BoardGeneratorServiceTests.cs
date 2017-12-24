using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperSolver.GameSimulator.Models;

namespace MinesweeperSolver.GameSimulator.Tests
{
    [TestClass]
    public class BoardGeneratorServiceTests
    {
        private const string Category = "MineGenerator";

        [TestMethod]
        [TestCategory(Category)]
        public void Generate_Single_Mine()
        {
            var generator = new BoardGeneratorService();

            var tiles = generator.Generate(1, 1, 1);

            Assert.AreEqual(1, tiles.GetLength(0));
            Assert.AreEqual(1, tiles.GetLength(1));
            Assert.IsTrue(tiles[0, 0]);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Generate_Multiple_Mines()
        {
            const int MineCount = 10;

            var generator = new BoardGeneratorService();

            var tiles = generator.Generate(9, 9, MineCount);

            var emptyTileCount = 0;
            var minedTileCount = 0;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j])
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
            Assert.AreEqual((tiles.GetLength(0) * tiles.GetLength(1)) - MineCount, emptyTileCount);
        }
    }
}