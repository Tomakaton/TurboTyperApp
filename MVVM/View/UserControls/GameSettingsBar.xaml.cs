using System.Windows.Controls;
using TurboTyper.MVVM.ViewModel;

namespace TurboTyper.MVVM.View.UserControls;

public partial class GameSettingsBar : UserControl
{
    public GameSettingsBar()
    {
        InitializeComponent();
        DataContext = new GameSettingsViewModel();
    }
}