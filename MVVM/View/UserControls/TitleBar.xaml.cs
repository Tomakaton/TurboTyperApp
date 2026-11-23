using System.Windows;
using System.Windows.Controls;

namespace TurboTyper.MVVM.View.UserControls;

public partial class TitleBar : UserControl
{
    public TitleBar()
    {
        InitializeComponent();
    }

    private void Minimize_OnClick(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        if (window is not null)
        {
            window.WindowState = WindowState.Minimized;
        }
    }

    private void Maximize_OnClick(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        if (window is not null)
        {
            window.WindowState = window.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
    }

    private void CloseWindow_OnClick(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this)?.Close();
    }
}
