using System.Windows;
using System.Windows.Input;
using TurboTyper.MVVM.View.Pages;
using TurboTyper.MVVM.ViewModel;

namespace TurboTyper.MVVM.View;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        MainFrame.Navigate(new LaunchPage());
    }


    private void TitelBar_Drag(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
            DragMove();
    }
}