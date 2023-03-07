using MDT;
using System.Windows;

namespace MDT_Step2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new ViewModel();

            DataContext = _viewModel;
        }
    }
}
