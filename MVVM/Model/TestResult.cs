using System;
using TurboTyper.MVVM.ViewModel.Enums;

namespace TurboTyper.MVVM.Model;

/// <summary>
/// Captures the final statistics of a completed typing session.
/// </summary>
public sealed record TestResult(
    GameMode Mode,
    int SelectedDifficulty,
    TimeSpan Elapsed,
    double NetWpm,
    double RawWpm,
    double Accuracy,
    int CorrectChars,
    int IncorrectChars,
    int SkippedChars,
    int CompletedWords,
    int TotalKeystrokes);
