﻿using MinesweeperSolver.DesctopApplication.ViewModels;
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
        public MinesweeperTile(GameProgress progress)
        {
            InitializeComponent();

            Model = new TileViewModel(progress);

            this.DataContext = Model;
        }

        public TileViewModel Model { get; }

        public MinesweeperTile(GameProgress progress, byte surroundingBombCount)
        {
            InitializeComponent();

            Model = new TileViewModel(progress, surroundingBombCount);

            this.DataContext = Model;
        }

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
