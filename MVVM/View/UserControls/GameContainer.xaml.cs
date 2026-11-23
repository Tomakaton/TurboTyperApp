using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TurboTyper.MVVM.ViewModel;

namespace TurboTyper.MVVM.View.UserControls;

public partial class GameContainer : UserControl
{
    private GameViewModel? _subscribedGameVm;

    public GameContainer()
    {
        InitializeComponent();
    }

    private GameViewModel? Vm => (DataContext as GameSessionViewModel)?.Game;

    private void GameContainer_Loaded(object sender, RoutedEventArgs e)
    {
        HookGameEvents(Vm);
        FocusTypingSurface();
    }

    private void GameContainer_Unloaded(object sender, RoutedEventArgs e)
    {
        HookGameEvents(null);
    }

    private void GameContainer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (IsLoaded)
        {
            HookGameEvents(Vm);
            FocusTypingSurface();
        }
    }

    private void HookGameEvents(GameViewModel? newGameVm)
    {
        if (ReferenceEquals(_subscribedGameVm, newGameVm)) return;

        if (_subscribedGameVm is not null)
        {
            _subscribedGameVm.GameReset -= OnGameReset;
        }

        _subscribedGameVm = newGameVm;

        if (_subscribedGameVm is not null)
        {
            _subscribedGameVm.GameReset += OnGameReset;
        }
    }

    private void OnGameReset(object? sender, EventArgs e)
    {
        FocusTypingSurface();
    }

    private void FocusTypingSurface()
    {
        Dispatcher.BeginInvoke(() => Keyboard.Focus(this), DispatcherPriority.Input);
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Keyboard.Focus(this);
    }

    private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (Vm is null) return;

        foreach (char c in e.Text)
        {
            Vm.ProcessCharInput(c);
        }

        e.Handled = true;
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (Vm is null) return;

        switch (e.Key)
        {
            case Key.Space:
                Vm.HandleSpace();
                e.Handled = true;
                break;

            case Key.Back:
                bool isCtrlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
                Vm.HandleBackspace(isCtrlPressed);
                e.Handled = true;
                break;

            case Key.Escape:
                Vm.ResetGame();
                e.Handled = true;
                break;
        }
    }
}
