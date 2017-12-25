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

            var blew = false;
            var board = new Board(1, 1, 1, generator, () => blew = true, (x, y) => { }, s => { });
            board.OpenTile(0, 0);

            Assert.IsTrue(blew);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_1x1_Without_Mine()
        {
            var tiles = new bool[,] { { false } };

            var generator = Substitute.For<IBoardGeneratorService>();
            generator.Generate(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(tiles);

            var blew = false;
            var board = new Board(1, 1, 1, generator, () => blew = true, (x, y) => { }, s => { });
            board.OpenTile(0, 0);

            Assert.IsFalse(blew);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_1x1_Out_Of_Bounds()
        {
            var tiles = new bool[,] { { false } };

            var generator = Substitute.For<IBoardGeneratorService>();
            generator.Generate(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(tiles);

            var board = new Board(1, 1, 1, generator, () => { }, (x, y) => { }, s => { });

            Assert.IsFalse(board.IsInBoard(-1, -1));
            Assert.IsFalse(board.IsInBoard(-1, 0));
            Assert.IsFalse(board.IsInBoard(-1, 1));
            Assert.IsFalse(board.IsInBoard(0, -1));
            Assert.IsFalse(board.IsInBoard(0, 1));
            Assert.IsFalse(board.IsInBoard(1, -1));
            Assert.IsFalse(board.IsInBoard(1, 0));
            Assert.IsFalse(board.IsInBoard(1, 1));
        }

        //[TestMethod]
        //[TestCategory(Category)]
        //public void Board_1x1_Count_Mines_No_Mines()
        //{
        //    var tiles = new bool[,] { { false } };

        //    var generator = Substitute.For<IBoardGeneratorService>();
        //    generator.Generate(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(tiles);

        //    var blew = false;
        //    var board = new Board(1, 1, 1, generator, () => blew = true);

        //    Assert.AreEqual(0, board.SuroundingMines(0, 0));
        //}

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

            var blew = false;
            var board = new Board(3, 3, 9, generator, () => blew = true, (x, y) => { }, s => { });

            board.OpenTile(0, 0);
            Assert.IsFalse(blew);
            board.OpenTile(0, 1);
            Assert.IsFalse(blew);
            board.OpenTile(0, 2);
            Assert.IsFalse(blew);
            board.OpenTile(1, 0);
            Assert.IsFalse(blew);
            board.OpenTile(1, 2);
            Assert.IsFalse(blew);
            board.OpenTile(2, 0);
            Assert.IsFalse(blew);
            board.OpenTile(2, 1);
            Assert.IsFalse(blew);
            board.OpenTile(2, 2);
            Assert.IsFalse(blew);
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
            generator.Generate(3, 3, 9).Returns(tiles);

            var blew = false;
            var board = new Board(3, 3, 9, generator, () => blew = true, (x, y) => { }, s => { });

            board.OpenTile(1, 1);
            Assert.IsFalse(blew);
        }
    }
}