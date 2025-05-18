using System.Collections.ObjectModel;
using TurboTyper.Core;
using TurboTyper.MVVM.ViewModel.Enums;

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
        var newValues = Mode == GameMode.Time 
            ? new List<int> { 30, 60, 120 }
            : new List<int> { 10, 30, 50 };

        Difficulties.Clear();
        foreach (var val in newValues)
        {
            Difficulties.Add(val); // Manually triggers collection changed
        }
    }
}
