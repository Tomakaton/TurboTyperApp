using System.Collections.Generic;
using System.Collections.ObjectModel;
using TurboTyper.Core;
using TurboTyper.MVVM.ViewModel.Enums;

namespace TurboTyper.MVVM.ViewModel;

public class GameSettingsViewModel : ObservableObject
{
    private GameMode _mode = GameMode.Time;
    private ObservableCollection<DifficultyOption> _difficulties = new();
    private int _selectedDifficulty;

    public GameSettingsViewModel()
    {
        UpdateDifficulties();
    }

    public GameMode Mode
    {
        get => _mode;
        set
        {
            if (SetField(ref _mode, value))
            {
                UpdateDifficulties();
            }
        }
    }

    public ObservableCollection<DifficultyOption> Difficulties
    {
        get => _difficulties;
        set => SetField(ref _difficulties, value);
    }

    public int SelectedDifficulty
    {
        get => _selectedDifficulty;
        set
        {
            if (SetField(ref _selectedDifficulty, value))
            {
                foreach (var diff in Difficulties)
                {
                    diff.IsSelected = diff.Value == value;
                }
            }
        }
    }

    private void UpdateDifficulties()
    {
        List<int> newValues = Mode == GameMode.Time
            ? [30, 60, 120]
            : [10, 30, 50];

        int defaultDifficulty = newValues[0];
        Difficulties.Clear();

        foreach (int val in newValues)
        {
            var option = new DifficultyOption
            {
                Value = val,
                IsSelected = val == defaultDifficulty
            };
            option.Selected += (s, _) => SelectedDifficulty = ((DifficultyOption)s!).Value;
            Difficulties.Add(option);
        }

        SelectedDifficulty = defaultDifficulty;
    }
}
