using System.Windows;
using TurboTyper.MVVM.ViewModel;

namespace TurboTyper.MVVM.View.Windows;

public partial class GameWindow : Window
{
    public GameWindow()
    {
        InitializeComponent();
        DataContext = new GameSessionViewModel();
    }
}