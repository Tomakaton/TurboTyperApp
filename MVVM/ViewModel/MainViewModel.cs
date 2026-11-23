using TurboTyper.Core;
using TurboTyper.MVVM.Model;

namespace TurboTyper.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    private readonly LaunchViewModel _launchViewModel;
    private readonly GameSessionViewModel _gameSession;
    private ObservableObject _currentView;

    public MainViewModel()
    {
        _launchViewModel = new LaunchViewModel(StartGame);

        _gameSession = new GameSessionViewModel();
        _gameSession.GameCompleted += OnGameCompleted;
        _gameSession.HomeRequested += (_, _) => GoHome();

        _currentView = _launchViewModel;
    }

    public ObservableObject CurrentView
    {
        get => _currentView;
        private set => SetField(ref _currentView, value);
    }

    private void StartGame()
    {
        _gameSession.Game.ResetGame();
        CurrentView = _gameSession;
    }

    private void OnGameCompleted(TestResult result)
    {
        CurrentView = new ResultsViewModel(result, StartGame, GoHome);
    }

    private void GoHome()
    {
        _gameSession.Game.ResetGame();
        CurrentView = _launchViewModel;
    }
}
