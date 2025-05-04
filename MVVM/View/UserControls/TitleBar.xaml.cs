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
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }

    private void Maximize_OnClick(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow != null)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }
    }

    private void CloseWindow_OnClick(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.Close();
        
    }
}