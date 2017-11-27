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

            const byte ColCount = 30;
            const byte RowCount = 20;
            const int BombCount = 60;

            // Create Columns
            for (int i = 0; i < ColCount; i++)
            {
                var gridCol = new ColumnDefinition { Width = new GridLength(30) };
                LayoutRoot.ColumnDefinitions.Add(gridCol);
            }

            // Create Rows
            for (int i = 0; i < RowCount; i++)
            {
                var gridRow = new RowDefinition { Height = new GridLength(30) };
                LayoutRoot.RowDefinitions.Add(gridRow);
            }

            LayoutRoot.Width = 30 * ColCount;
            LayoutRoot.Height = 30 * RowCount;

            var board = new Board(ColCount, RowCount);
            var generator = new MineGenerator();
            generator.Fill(board, BombCount);

            // Stop game when blow up
            // Open when zero is pressed

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

                    Grid.SetColumn(tile, i);
                    Grid.SetRow(tile, j);

                    LayoutRoot.Children.Add(tile);
                }
            }
        }
    }
}
