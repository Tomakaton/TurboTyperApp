using System.Windows;
using TurboTyper.MVVM.ViewModel;

namespace TurboTyper.MVVM.View;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
