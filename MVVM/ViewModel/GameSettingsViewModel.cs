using System.Collections.ObjectModel;
using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class GameSettingsViewModel : ObservableObject
{
    private GameMode _mode = GameMode.Time;
    

    public GameSettingsViewModel()
    {
        UpdateDifficulty();
    }

    public GameMode Mode
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
        if (Mode == GameMode.Time)
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

    public enum GameMode
    {
        Time,
        Words
    }
}
