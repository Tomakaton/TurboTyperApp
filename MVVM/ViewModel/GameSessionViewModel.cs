using System;
using System.Windows.Input;
using TurboTyper.Core;
using TurboTyper.MVVM.Model;

namespace TurboTyper.MVVM.ViewModel;

public class GameSessionViewModel : ObservableObject
{
    public GameSettingsViewModel Settings { get; }
    public GameViewModel Game { get; }
    public ICommand RestartCommand => Game.RestartCommand;
    public ICommand HomeCommand { get; }

    public event Action<TestResult>? GameCompleted;
    public event EventHandler? HomeRequested;

    public GameSessionViewModel()
    {
        Settings = new GameSettingsViewModel();
        Game = new GameViewModel
        {
            Mode = Settings.Mode,
            SelectedDifficulty = Settings.SelectedDifficulty
        };

        HomeCommand = new RelayCommand(() => HomeRequested?.Invoke(this, EventArgs.Empty));
        Game.GameCompleted += result => GameCompleted?.Invoke(result);

        Settings.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(GameSettingsViewModel.SelectedDifficulty))
            {
                Game.SelectedDifficulty = Settings.SelectedDifficulty;
            }
            else if (e.PropertyName == nameof(GameSettingsViewModel.Mode))
            {
                Game.Mode = Settings.Mode;
            }
        };
    }
}
