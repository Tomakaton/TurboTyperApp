using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class GameSessionViewModel : ObservableObject
{
    public GameSettingsViewModel Settings { get; }
    public GameViewModel Game { get; }
    
    public GameSessionViewModel()
    {
        Settings = new GameSettingsViewModel();
        Game = new GameViewModel();
        
        // Connect settings changes to game
        Settings.PropertyChanged += (s, e) => 
        {
            if(e.PropertyName == nameof(GameSettingsViewModel.SelectedDifficulty))
            {
                Game.SelectedDifficulty = Settings.SelectedDifficulty;
            } 
            else if(e.PropertyName == nameof(GameSettingsViewModel.Mode))
            {
                Game.Mode = Settings.Mode;
            }
            
        };
    }
}