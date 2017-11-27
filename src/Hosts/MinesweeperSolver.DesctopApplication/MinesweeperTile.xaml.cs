using MinesweeperSolver.DesctopApplication.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace MinesweeperSolver.DesctopApplication
{
    /// <summary>
    /// Interaction logic for MinesweeperTile.xaml
    /// </summary>
    public partial class MinesweeperTile : UserControl
    {
        private TileViewModel _model;

        public MinesweeperTile()
        {
            InitializeComponent();

            _model = new TileViewModel();

            this.DataContext = _model;
        }

        public MinesweeperTile(byte surroundingBombCount)
        {
            InitializeComponent();

            _model = new TileViewModel(surroundingBombCount);

            this.DataContext = _model;
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _model.Open();
        }

        private void Rectangle_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _model.Flag();
        }
    }
}
