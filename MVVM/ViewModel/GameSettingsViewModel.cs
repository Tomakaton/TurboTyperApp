using System.Collections.ObjectModel;
using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class GameSettingsViewModel : ObsarvableObject
{
    private string _mode;

    public string Mode
    {
        get => _mode;
        set
        {
            if (SetField(ref _mode , value))
            {
                UpdateDifficulty();
            }
        }
    }

    private ObservableCollection<string> _difficulties = new();

    public ObservableCollection<string> Difficulties
    {
        get => _difficulties;
        set => SetField(ref _difficulties, value);
    }
    
    
    private void UpdateDifficulty()
    {
        Difficulties.Clear();
        if (Mode == "Time")
        {
            Difficulties.Add("30s");
            Difficulties.Add("60s");
            Difficulties.Add("120s");
        }
        else
        {
            Difficulties.Add("10");
            Difficulties.Add("30");
            Difficulties.Add("50");
        }
    }
}
