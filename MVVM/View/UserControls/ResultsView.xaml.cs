using System.Windows;
using System.Windows.Controls;

namespace TurboTyper.MVVM.View.UserControls;

public partial class ResultsView : UserControl
{
    public ResultsView()
    {
        InitializeComponent();
    }

    private void ResultsView_Loaded(object sender, RoutedEventArgs e)
    {
        PlayAgainButton.Focus();
    }
}
