using FluentNewsApp.ViewModels;
using System.Windows;

namespace FluentNewsApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
        }
    }
}
