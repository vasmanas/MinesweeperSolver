using MinesweeperSolver.DesctopApplication.ViewModels;
using MinesweeperSolver.GameSimulator.Models;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace MinesweeperSolver.DesctopApplication
{
    /// <summary>
    /// Interaction logic for MinesweeperTile.xaml
    /// </summary>
    public partial class MinesweeperTile : UserControl
    {
        public MinesweeperTile(Board board, int x, int y, GameProgress progress)
        {
            InitializeComponent();

            Model = new TileViewModel(board, x, y, progress);

            this.DataContext = Model;
        }

        public TileViewModel Model { get; }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Model.Open();
        }

        private void Rectangle_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Model.Flag();
        }
    }
}
