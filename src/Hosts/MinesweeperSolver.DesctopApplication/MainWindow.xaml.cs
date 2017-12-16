﻿using MinesweeperSolver.DesctopApplication.ViewModels;
using MinesweeperSolver.GameSimulator;
using MinesweeperSolver.GameSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinesweeperSolver.DesctopApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            const byte ColCnt = 30;
            const byte RowCnt = 20;
            const int BombCnt = 150;

            ColumnCount.Text = ColCnt.ToString();
            RowCount.Text = RowCnt.ToString();
            BombCount.Text = BombCnt.ToString();

            Restart(ColCnt, RowCnt, BombCnt);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Statistics.Content = string.Empty;

            MineField.ColumnDefinitions.Clear();
            MineField.RowDefinitions.Clear();
            MineField.Children.Clear();

            var ccnt = byte.Parse(ColumnCount.Text);
            var rcnt = byte.Parse(RowCount.Text);
            var bcnt = int.Parse(BombCount.Text);

            Restart(ccnt, rcnt, bcnt);
        }

        private void PositiveInteger_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsPositiveInteger(e.Text);
        }

        private void Restart(byte colCount, byte rowCount, int mineCount)
        {
            const double ColumWidth = 30;
            const double RowHeight = 30;

            // Create Columns
            for (int i = 0; i < colCount; i++)
            {
                var gridCol = new ColumnDefinition { Width = new GridLength(ColumWidth) };
                MineField.ColumnDefinitions.Add(gridCol);
            }

            // Create Rows
            for (int i = 0; i < rowCount; i++)
            {
                var gridRow = new RowDefinition { Height = new GridLength(RowHeight) };
                MineField.RowDefinitions.Add(gridRow);
            }

            MineField.Width = ColumWidth * colCount;
            MineField.Height = RowHeight * rowCount;

            var board = new Board(colCount, rowCount);
            var generator = new MineGenerator();
            generator.Fill(board, mineCount);

            var viewTiles = new MinesweeperTile[colCount, rowCount];

            void EndGame(string statistics)
            {
                OpenBoard(viewTiles, board);

                Statistics.Content = statistics;
            }

            var progress = new GameProgress(colCount * rowCount, mineCount, EndGame);

            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    var tile =
                        board.IsMineAt(i, j) ?
                        new MinesweeperTile(progress) :
                        new MinesweeperTile(progress, (byte)board.SuroundingMines(i, j));

                    viewTiles[i, j] = tile;

                    Grid.SetColumn(tile, i);
                    Grid.SetRow(tile, j);

                    MineField.Children.Add(tile);
                }
            }

            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    if (board.IsMineAt(i, j))
                    {
                        continue;
                    }

                    if (board.SuroundingMines(i, j) != 0)
                    {
                        continue;
                    }

                    var model = viewTiles[i, j].Model;

                    // Add neighbours
                    if (i > 0)
                    {
                        if (j > 0)
                        {
                            model.Addneighbour(viewTiles[i - 1, j - 1].Model);
                        }

                        model.Addneighbour(viewTiles[i - 1, j].Model);

                        if (j < rowCount - 1)
                        {
                            model.Addneighbour(viewTiles[i - 1, j + 1].Model);
                        }
                    }

                    if (j > 0)
                    {
                        model.Addneighbour(viewTiles[i, j - 1].Model);
                    }

                    if (j < rowCount - 1)
                    {
                        model.Addneighbour(viewTiles[i, j + 1].Model);
                    }

                    if (i < colCount - 1)
                    {
                        if (j > 0)
                        {
                            model.Addneighbour(viewTiles[i + 1, j - 1].Model);
                        }

                        model.Addneighbour(viewTiles[i + 1, j].Model);

                        if (j < rowCount - 1)
                        {
                            model.Addneighbour(viewTiles[i + 1, j + 1].Model);
                        }
                    }
                }
            }
        }

        private void OpenBoard(MinesweeperTile[,] viewTiles, Board board)
        {
            var colCount = viewTiles.GetLength(0);
            var rowCount = viewTiles.GetLength(1);

            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    var vm = viewTiles[i, j].Model;

                    if (board.IsMineAt(i, j))
                    {
                        if (vm.Hidden)
                        {
                            vm.Flag();
                        }
                    }
                    else
                    {
                        if (!vm.Uncovered)
                        {
                            vm.Open();
                        }
                    }
                }
            }
        }

        private bool IsPositiveInteger(string text)
        {
            var regex = new Regex("[0-9]+");
            return regex.IsMatch(text);
        }
    }
}
