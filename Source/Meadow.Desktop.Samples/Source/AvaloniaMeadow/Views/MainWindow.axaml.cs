using Avalonia.Controls;
using AvaloniaMeadow.ViewModels;

namespace AvaloniaMeadow.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}