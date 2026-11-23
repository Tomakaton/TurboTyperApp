using System;
using System.Windows.Input;
using TurboTyper.Core;
using TurboTyper.MVVM.Model;
using TurboTyper.MVVM.ViewModel.Enums;

namespace TurboTyper.MVVM.ViewModel;

public class ResultsViewModel : ObservableObject
{
    public ResultsViewModel(TestResult result, Action? onPlayAgain = null, Action? onHome = null)
    {
        Result = result;
        PlayAgainCommand = new RelayCommand(() => onPlayAgain?.Invoke());
        HomeCommand = new RelayCommand(() => onHome?.Invoke());
    }

    public TestResult Result { get; }
    public ICommand PlayAgainCommand { get; }
    public ICommand HomeCommand { get; }

    public string NetWpmDisplay => $"{Result.NetWpm:F0}";
    public string RawWpmDisplay => $"{Result.RawWpm:F0}";
    public string AccuracyDisplay => $"{Result.Accuracy:F1}%";

    public string ModeSummaryDisplay => Result.Mode == GameMode.Time
        ? $"Time  •  {Result.SelectedDifficulty}s"
        : $"Words  •  {Result.SelectedDifficulty} words";

    public string CharacterBreakdownDisplay =>
        $"{Result.CorrectChars} / {Result.IncorrectChars} / {Result.SkippedChars}";

    public string DurationDisplay => $"{Result.Elapsed.TotalSeconds:F1}s";
}
