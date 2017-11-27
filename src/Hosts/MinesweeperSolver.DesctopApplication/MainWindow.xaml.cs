using MinesweeperSolver.GameSimulator;
using MinesweeperSolver.GameSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            Restart();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MineField.ColumnDefinitions.Clear();
            MineField.RowDefinitions.Clear();
            MineField.Children.Clear();

            Restart();
        }

        private void Restart()
        {
            const byte ColCount = 30;
            const byte RowCount = 20;
            const int BombCount = 150;

            // Create Columns
            for (int i = 0; i < ColCount; i++)
            {
                var gridCol = new ColumnDefinition { Width = new GridLength(30) };
                MineField.ColumnDefinitions.Add(gridCol);
            }

            // Create Rows
            for (int i = 0; i < RowCount; i++)
            {
                var gridRow = new RowDefinition { Height = new GridLength(30) };
                MineField.RowDefinitions.Add(gridRow);
            }

            MineField.Width = 30 * ColCount;
            MineField.Height = 30 * RowCount;

            var board = new Board(ColCount, RowCount);
            var generator = new MineGenerator();
            generator.Fill(board, BombCount);

            // TODO: Open when zero is pressed
            // TODO: Stop game when blow up

            var viewTiles = new MinesweeperTile[ColCount, RowCount];

            for (int i = 0; i < ColCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    MinesweeperTile tile;

                    if (board.IsMineAt(i, j))
                    {
                        tile = new MinesweeperTile();
                    }
                    else
                    {
                        tile = new MinesweeperTile((byte)board.SuroundingMines(i, j));
                    }

                    viewTiles[i, j] = tile;

                    Grid.SetColumn(tile, i);
                    Grid.SetRow(tile, j);

                    MineField.Children.Add(tile);
                }
            }

            for (int i = 0; i < ColCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
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

                        if (j < RowCount - 1)
                        {
                            model.Addneighbour(viewTiles[i - 1, j + 1].Model);
                        }
                    }

                    if (j > 0)
                    {
                        model.Addneighbour(viewTiles[i, j - 1].Model);
                    }

                    if (j < RowCount - 1)
                    {
                        model.Addneighbour(viewTiles[i, j + 1].Model);
                    }

                    if (i < ColCount - 1)
                    {
                        if (j > 0)
                        {
                            model.Addneighbour(viewTiles[i + 1, j - 1].Model);
                        }

                        model.Addneighbour(viewTiles[i + 1, j].Model);

                        if (j < RowCount - 1)
                        {
                            model.Addneighbour(viewTiles[i + 1, j + 1].Model);
                        }
                    }
                }
            }
        }
    }
}
