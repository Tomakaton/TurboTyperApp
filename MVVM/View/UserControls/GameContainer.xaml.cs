using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TurboTyper.MVVM.ViewModel;

namespace TurboTyper.MVVM.View.UserControls;

public partial class GameContainer : UserControl
{
    private GameSessionViewModel SessionVm => (GameSessionViewModel)DataContext;
    private GameViewModel Vm => SessionVm.Game;
    public GameContainer()
    {
        InitializeComponent();
    }

    private void GameContainer_Loaded(object sender, RoutedEventArgs e)
    {
        Keyboard.Focus(this);
    }

    private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // e.Text will be the character(s) typed
        foreach (var c in e.Text)
        {
            Vm.ProcessInput(c);
        }
        // mark handled so the TextBox (if any) doesn’t also consume it
        e.Handled = true;
    }
}