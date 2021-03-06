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
            const int BombCnt = 120;

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

            var generator = new BoardGeneratorService();
            var board =
                new BoardViewModel(
                    colCount,
                    rowCount,
                    mineCount,
                    generator,
                    MineField);

            var viewTiles = new MinesweeperTile[colCount, rowCount];

            var progress = new GameProgress(board, s => Statistics.Content = s);

            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    var viewTile = new MinesweeperTile(board, i, j, progress);

                    viewTiles[i, j] = viewTile;

                    Grid.SetColumn(viewTile, i);
                    Grid.SetRow(viewTile, j);

                    MineField.Children.Add(viewTile);
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
