using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperSolver.GameSimulator.Models;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolver.GameSimulator.Tests
{
    [TestClass]
    public class BoardTests
    {
        private const string Category = "Board";

        [TestMethod]
        [TestCategory(Category)]
        public void Board_1x1_With_Mine()
        {
            var tiles = new bool[,] { { true } };

            var generator = Substitute.For<IBoardGeneratorService>();
            generator.Generate(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(tiles);

            var board = new Board(1, 1, 1, generator);
            board.OpenTile(0, 0);

            Assert.AreEqual(State.Lost, board.EndOfGame);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_1x1_Without_Mine()
        {
            var tiles = new bool[,] { { false } };

            var generator = Substitute.For<IBoardGeneratorService>();
            generator.Generate(1, 1, 0).Returns(tiles);

            var board = new Board(1, 1, 0, generator);
            board.OpenTile(0, 0);

            Assert.AreEqual(State.Won, board.EndOfGame);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_1x1_Out_Of_Bounds()
        {
            var tiles = new bool[,] { { false } };

            var generator = Substitute.For<IBoardGeneratorService>();
            generator.Generate(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(tiles);

            var board = new Board(1, 1, 1, generator);

            Assert.IsFalse(board.IsOnBoard(-1, -1));
            Assert.IsFalse(board.IsOnBoard(-1, 0));
            Assert.IsFalse(board.IsOnBoard(-1, 1));
            Assert.IsFalse(board.IsOnBoard(0, -1));
            Assert.IsFalse(board.IsOnBoard(0, 1));
            Assert.IsFalse(board.IsOnBoard(1, -1));
            Assert.IsFalse(board.IsOnBoard(1, 0));
            Assert.IsFalse(board.IsOnBoard(1, 1));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_1x1_Count_Mines_No_Mines()
        {
            var tiles = new bool[,] { { false } };

            var generator = Substitute.For<IBoardGeneratorService>();
            generator.Generate(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(tiles);

            var board = new Board(1, 1, 1, generator);
            board.OpenTile(0, 0);


            Assert.AreEqual(0, board.SurroundingMineCount(0, 0));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_3x3_Count_Mines_No_Mines()
        {
            var tiles = new bool[,]
            {
                { false, false, false },
                { false, true, false },
                { false, false, false }
            };

            var generator = Substitute.For<IBoardGeneratorService>();
            generator.Generate(3, 3, 9).Returns(tiles);

            var board = new Board(3, 3, 9, generator);

            board.OpenTile(0, 0);
            Assert.AreEqual(State.Playing, board.EndOfGame);
            board.OpenTile(0, 1);
            Assert.AreEqual(State.Playing, board.EndOfGame);
            board.OpenTile(0, 2);
            Assert.AreEqual(State.Playing, board.EndOfGame);
            board.OpenTile(1, 0);
            Assert.AreEqual(State.Playing, board.EndOfGame);
            board.OpenTile(1, 2);
            Assert.AreEqual(State.Playing, board.EndOfGame);
            board.OpenTile(2, 0);
            Assert.AreEqual(State.Playing, board.EndOfGame);
            board.OpenTile(2, 1);
            Assert.AreEqual(State.Playing, board.EndOfGame);
            board.OpenTile(2, 2);
            Assert.AreEqual(State.Playing, board.EndOfGame);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_3x3_Count_Mines_All_Mines()
        {
            var tiles = new bool[,]
            {
                { true, true, true },
                { true, false, true },
                { true, true, true }
            };

            var generator = Substitute.For<IBoardGeneratorService>();
            generator.Generate(3, 3, 8).Returns(tiles);

            var board = new Board(3, 3, 8, generator);

            board.OpenTile(1, 1);
            Assert.AreEqual(State.Won, board.EndOfGame);
        }
    }
}