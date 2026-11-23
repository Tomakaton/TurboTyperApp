using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using TurboTyper.Core;
using TurboTyper.MVVM.Model;
using TurboTyper.MVVM.ViewModel.Enums;

namespace TurboTyper.MVVM.ViewModel;

public class GameViewModel : ObservableObject
{
    private const int VisibleLineCount = 3;
    private const int MaxCharsPerLine = 52;

    private readonly Stopwatch _stopwatch = new();
    private readonly DispatcherTimer _uiTimer;
    private readonly Queue<string> _remainingLines = new();
    private readonly List<TypingLine> _archivedLines = new();

    private int _selectedDifficulty = 30;
    private GameMode _mode = GameMode.Time;
    private ObservableCollection<TypingLine> _gameText = new();
    private TimeSpan _elapsed;

    private int _currentLine;
    private int _currentWordIndex;
    private int _currentCharIndex;
    private int _completedWords;
    private int _spacesPressed;
    private int _totalPrintableKeystrokes;
    private int _correctKeystrokes;
    private bool _isCompleted;

    private ObservableChar? _caretChar;
    private ObservableWord? _caretWordEnd;

    public GameViewModel()
    {
        RestartCommand = new RelayCommand(ResetGame);

        _uiTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _uiTimer.Tick += OnTimerTick;

        ResetGame();
    }

    public event Action<TestResult>? GameCompleted;
    public event EventHandler? GameReset;

    public ICommand RestartCommand { get; }

    public int SelectedDifficulty
    {
        get => _selectedDifficulty;
        set
        {
            if (SetField(ref _selectedDifficulty, value))
            {
                ResetGame();
            }
        }
    }

    public GameMode Mode
    {
        get => _mode;
        set
        {
            if (SetField(ref _mode, value))
            {
                ResetGame();
            }
        }
    }

    public ObservableCollection<TypingLine> GameText
    {
        get => _gameText;
        set => SetField(ref _gameText, value);
    }

    public TimeSpan Elapsed
    {
        get => _elapsed;
        private set
        {
            if (SetField(ref _elapsed, value))
            {
                OnPropertyChanged(nameof(PrimaryHudText));
                OnPropertyChanged(nameof(SecondaryHudText));
            }
        }
    }

    public string PrimaryHudText
    {
        get
        {
            if (Mode == GameMode.Time)
            {
                double remainingSeconds = Math.Max(0, SelectedDifficulty - Elapsed.TotalSeconds);
                int displaySeconds = _stopwatch.IsRunning
                    ? (int)Math.Ceiling(remainingSeconds)
                    : SelectedDifficulty;
                var displaySpan = TimeSpan.FromSeconds(displaySeconds);
                return $"{(int)displaySpan.TotalMinutes}:{displaySpan.Seconds:D2}";
            }

            return $"{_completedWords} / {SelectedDifficulty}";
        }
    }

    public string SecondaryHudText
    {
        get
        {
            if (Mode == GameMode.Time)
            {
                return _completedWords == 1 ? "1 word" : $"{_completedWords} words";
            }

            return $"{(int)Elapsed.TotalMinutes}:{Elapsed.Seconds:D2}";
        }
    }

    public void ResetGame()
    {
        _stopwatch.Reset();
        _uiTimer.Stop();

        _isCompleted = false;
        _currentLine = 0;
        _currentWordIndex = 0;
        _currentCharIndex = 0;
        _completedWords = 0;
        _spacesPressed = 0;
        _totalPrintableKeystrokes = 0;
        _correctKeystrokes = 0;

        ClearCaret();
        _archivedLines.Clear();
        _remainingLines.Clear();
        GameText.Clear();

        if (Mode == GameMode.Time)
        {
            for (int i = 0; i < VisibleLineCount; i++)
            {
                GameText.Add(new TypingLine(WordProvider.GenerateLine(MaxCharsPerLine)));
            }
        }
        else
        {
            List<string> lines = WordProvider.GenerateLinesForWordBudget(SelectedDifficulty, MaxCharsPerLine);
            foreach (string line in lines)
            {
                if (GameText.Count < VisibleLineCount)
                {
                    GameText.Add(new TypingLine(line));
                }
                else
                {
                    _remainingLines.Enqueue(line);
                }
            }
        }

        Elapsed = TimeSpan.Zero;
        OnPropertyChanged(nameof(PrimaryHudText));
        OnPropertyChanged(nameof(SecondaryHudText));
        UpdateCaret();
        GameReset?.Invoke(this, EventArgs.Empty);
    }

    public void ProcessCharInput(char c)
    {
        if (_isCompleted || char.IsControl(c) || c == ' ') return;
        if (_currentLine >= GameText.Count) return;

        EnsureTimerStarted();

        var currentLine = GameText[_currentLine];
        if (_currentWordIndex >= currentLine.Text.Count) return;

        var currentWord = currentLine.Text[_currentWordIndex];
        if (_currentCharIndex >= currentWord.Word.Count)
        {
            // Clamp at word end until user presses Space or Backspace
            return;
        }

        var targetChar = currentWord.Word[_currentCharIndex];
        _totalPrintableKeystrokes++;

        if (c == targetChar.Letter)
        {
            targetChar.State = CharacterState.Correct;
            _correctKeystrokes++;
        }
        else
        {
            targetChar.State = CharacterState.Incorrect;
        }

        _currentCharIndex++;
        UpdateCaret();

        if (Mode == GameMode.Words &&
            _remainingLines.Count == 0 &&
            _currentLine == GameText.Count - 1 &&
            _currentWordIndex == currentLine.Text.Count - 1 &&
            _currentCharIndex == currentWord.Word.Count)
        {
            currentWord.IsWritten = true;
            _completedWords++;
            OnPropertyChanged(nameof(PrimaryHudText));
            CompleteGame();
        }
    }

    public void HandleSpace()
    {
        if (_isCompleted || !_stopwatch.IsRunning || _currentCharIndex == 0) return;
        if (_currentLine >= GameText.Count) return;

        var currentLine = GameText[_currentLine];
        if (_currentWordIndex >= currentLine.Text.Count) return;

        var currentWord = currentLine.Text[_currentWordIndex];
        for (int i = _currentCharIndex; i < currentWord.Word.Count; i++)
        {
            if (currentWord.Word[i].State == CharacterState.NotTyped)
            {
                currentWord.Word[i].State = CharacterState.Skipped;
            }
        }

        currentWord.IsWritten = true;
        _completedWords++;
        _spacesPressed++;
        OnPropertyChanged(nameof(PrimaryHudText));
        OnPropertyChanged(nameof(SecondaryHudText));

        if (Mode == GameMode.Words && _completedWords >= SelectedDifficulty)
        {
            CompleteGame();
            return;
        }

        _currentWordIndex++;
        _currentCharIndex = 0;

        if (_currentWordIndex >= currentLine.Text.Count)
        {
            _currentWordIndex = 0;
            AdvanceLine();
        }

        UpdateCaret();
    }

    public void HandleBackspace(bool isCtrlPressed)
    {
        if (_isCompleted || _currentCharIndex == 0) return;
        if (_currentLine >= GameText.Count) return;

        var currentLine = GameText[_currentLine];
        if (_currentWordIndex >= currentLine.Text.Count) return;

        var currentWord = currentLine.Text[_currentWordIndex];

        if (isCtrlPressed)
        {
            for (int i = 0; i < _currentCharIndex; i++)
            {
                currentWord.Word[i].State = CharacterState.NotTyped;
            }
            _currentCharIndex = 0;
        }
        else
        {
            _currentCharIndex--;
            currentWord.Word[_currentCharIndex].State = CharacterState.NotTyped;
        }

        UpdateCaret();
    }

    private void AdvanceLine()
    {
        if (_currentLine == 0)
        {
            _currentLine = 1;
            return;
        }

        if (Mode == GameMode.Time)
        {
            _archivedLines.Add(GameText[0]);
            GameText.RemoveAt(0);
            GameText.Add(new TypingLine(WordProvider.GenerateLine(MaxCharsPerLine)));
            _currentLine = 1;
        }
        else if (_remainingLines.Count > 0)
        {
            _archivedLines.Add(GameText[0]);
            GameText.RemoveAt(0);
            GameText.Add(new TypingLine(_remainingLines.Dequeue()));
            _currentLine = 1;
        }
        else
        {
            _currentLine++;
            if (_currentLine >= GameText.Count)
            {
                CompleteGame();
            }
        }
    }

    private void EnsureTimerStarted()
    {
        if (!_stopwatch.IsRunning)
        {
            _stopwatch.Start();
            _uiTimer.Start();
            OnPropertyChanged(nameof(PrimaryHudText));
        }
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        if (_isCompleted) return;

        Elapsed = _stopwatch.Elapsed;
        if (Mode == GameMode.Time && Elapsed >= TimeSpan.FromSeconds(SelectedDifficulty))
        {
            CompleteGame();
        }
    }

    private void CompleteGame()
    {
        if (_isCompleted) return;
        _isCompleted = true;

        _stopwatch.Stop();
        _uiTimer.Stop();
        ClearCaret();

        TimeSpan finalDuration = Mode == GameMode.Time
            ? TimeSpan.FromSeconds(SelectedDifficulty)
            : _stopwatch.Elapsed;

        Elapsed = finalDuration;

        int correctChars = _spacesPressed;
        int incorrectChars = 0;
        int skippedChars = 0;

        foreach (TypingLine line in _archivedLines.Concat(GameText))
        {
            foreach (ObservableWord word in line.Text)
            {
                foreach (ObservableChar ch in word.Word)
                {
                    switch (ch.State)
                    {
                        case CharacterState.Correct:
                            correctChars++;
                            break;
                        case CharacterState.Incorrect:
                            incorrectChars++;
                            break;
                        case CharacterState.Skipped:
                            skippedChars++;
                            break;
                    }
                }
            }
        }

        double minutes = finalDuration.TotalMinutes;
        int totalChars = correctChars + incorrectChars + skippedChars;
        double rawWpm = minutes > 1e-4 ? totalChars / (5.0 * minutes) : 0;
        double netWpm = minutes > 1e-4 ? correctChars / (5.0 * minutes) : 0;
        double accuracy = _totalPrintableKeystrokes > 0
            ? (_correctKeystrokes * 100.0) / _totalPrintableKeystrokes
            : 100.0;

        var result = new TestResult(
            Mode,
            SelectedDifficulty,
            finalDuration,
            netWpm,
            rawWpm,
            accuracy,
            correctChars,
            incorrectChars,
            skippedChars,
            _completedWords,
            _totalPrintableKeystrokes);

        GameCompleted?.Invoke(result);
    }

    private void UpdateCaret()
    {
        ClearCaret();

        if (_isCompleted || _currentLine >= GameText.Count) return;

        var currentLine = GameText[_currentLine];
        if (_currentWordIndex >= currentLine.Text.Count) return;

        var currentWord = currentLine.Text[_currentWordIndex];
        if (_currentCharIndex < currentWord.Word.Count)
        {
            _caretChar = currentWord.Word[_currentCharIndex];
            _caretChar.IsCurrent = true;
        }
        else
        {
            _caretWordEnd = currentWord;
            _caretWordEnd.IsCaretAtWordEnd = true;
        }
    }

    private void ClearCaret()
    {
        if (_caretChar is not null)
        {
            _caretChar.IsCurrent = false;
            _caretChar = null;
        }

        if (_caretWordEnd is not null)
        {
            _caretWordEnd.IsCaretAtWordEnd = false;
            _caretWordEnd = null;
        }
    }
}
