using System.Collections.ObjectModel;
using System.Windows.Threading;
using TurboTyper.Converters;
using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class GameSettingsViewModel : ObservableObject
{
    private GameMode _mode = GameMode.Time;
    
    public GameSettingsViewModel()
    {
        UpdateDifficulties();
        SelectedDifficulty = Difficulties[0];
    }
    
    public GameMode Mode
    {
        get => _mode;
        set
        {
            if (SetField(ref _mode , value))
            {
                UpdateDifficulties();
                SelectedDifficulty = Difficulties[0];
            }
        }
    }
    

    private  ObservableCollection<int> _difficulties = new();

    public ObservableCollection<int> Difficulties
    {
        get => _difficulties;
        set => SetField(ref _difficulties, value);
    }
    
    private int _selectedDifficulty;
    public int SelectedDifficulty
    {
        get => _selectedDifficulty;
        set => SetField(ref _selectedDifficulty, value);
    }
    
    private void UpdateDifficulties()
    {
        Difficulties.Clear();
        if (Mode == GameMode.Time)
        {
            Difficulties.Add(30);
            Difficulties.Add(60);
            Difficulties.Add(120);
        }
        else
        {
            Difficulties.Add(10);
            Difficulties.Add(30);
            Difficulties.Add(50);
        }
    }

    public enum GameMode
    {
        Time,
        Words
    }
    
}
