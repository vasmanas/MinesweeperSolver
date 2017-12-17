using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperSolver.GameSimulator.Models;
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
            var board = new Board(1, 1);
            board.Tiles[0, 0] = new Tile(true);

            Assert.IsTrue(board.IsMineAt(0, 0));            
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_1x1_Without_Mine()
        {
            var board = new Board(1, 1);
            board.Tiles[0, 0] = new Tile(false);

            Assert.IsFalse(board.IsMineAt(0, 0));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_1x1_Out_Of_Bounds()
        {
            var board = new Board(1, 1);
            board.Tiles[0, 0] = new Tile(false);

            Assert.IsFalse(board.IsMineAt(-1, -1));
            Assert.IsFalse(board.IsMineAt(-1, 0));
            Assert.IsFalse(board.IsMineAt(-1, 1));
            Assert.IsFalse(board.IsMineAt(0, -1));
            Assert.IsFalse(board.IsMineAt(0, 1));
            Assert.IsFalse(board.IsMineAt(1, -1));
            Assert.IsFalse(board.IsMineAt(1, 0));
            Assert.IsFalse(board.IsMineAt(1, 1));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_1x1_Count_Mines_No_Mines()
        {
            var board = new Board(1, 1);
            board.Tiles[0, 0] = new Tile(false);

            Assert.AreEqual(0, board.SuroundingMines(0, 0));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_9x9_Count_Mines_No_Mines()
        {
            var board = new Board(3, 3);
            board.Tiles[0, 1] = new Tile(false);
            board.Tiles[0, 2] = new Tile(false);
            board.Tiles[1, 0] = new Tile(false);
            board.Tiles[0, 0] = new Tile(false);
            board.Tiles[1, 1] = new Tile(false);
            board.Tiles[1, 2] = new Tile(false);
            board.Tiles[2, 0] = new Tile(false);
            board.Tiles[2, 1] = new Tile(false);
            board.Tiles[2, 2] = new Tile(false);

            Assert.AreEqual(0, board.SuroundingMines(1, 1));
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Board_9x9_Count_Mines_All_Mines()
        {
            var board = new Board(3, 3);
            board.Tiles[0, 0] = new Tile(true);
            board.Tiles[0, 1] = new Tile(true);
            board.Tiles[0, 2] = new Tile(true);
            board.Tiles[1, 0] = new Tile(true);
            board.Tiles[1, 1] = new Tile(true);
            board.Tiles[1, 2] = new Tile(true);
            board.Tiles[2, 0] = new Tile(true);
            board.Tiles[2, 1] = new Tile(true);
            board.Tiles[2, 2] = new Tile(true);
                                         
            Assert.AreEqual(8, board.SuroundingMines(1, 1));
        }
    }
}