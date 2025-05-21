using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Threading;
using TurboTyper.Core;
using TurboTyper.MVVM.ViewModel.Enums;

namespace TurboTyper.MVVM.ViewModel;

public class GameViewModel : ObservableObject
{
    private int _selectedDifficulty;
    private GameMode _mode;
    private ObservableCollection<TypingLine> _gameText = new();

    public GameViewModel()
    {
        SetGameText(3);

        // set up a DispatcherTimer to update Elapsed every 100ms
        _uiTimer = new DispatcherTimer {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _uiTimer.Tick += (_,_) => {
            Elapsed = _stopwatch.Elapsed;
        };
    }

    public int SelectedDifficulty
    {
        get => _selectedDifficulty;
        set => SetField(ref _selectedDifficulty, value);
    }

    public GameMode Mode
    {
        get => _mode;
        set => SetField(ref _mode, value);
    }

    public ObservableCollection<TypingLine> GameText
    {
        get => _gameText;
        set => SetField(ref _gameText, value);
    }

    private void SetGameText(int lines)
    {
        GameText.Clear();
        for (int i = 0; i < lines; i++)
        {
            GameText.Add(new TypingLine(GetRandomLine(58)));
        }
    }
    
    private static string GetRandomLine(int length, int maxCharsLength = 58)
    {
        string[] wordPool =
        [
            "apple", "banana", "cat", "dog", "elephant", "fish", "grape", "hat", "ice", "jungle",
            "kite", "lion", "monkey", "notebook", "orange", "pencil", "queen", "rabbit", "sun", "tree",
            "umbrella", "vase", "whale", "xylophone", "yarn", "zebra", "book", "car", "door", "egg",
            "frog", "guitar", "house", "island", "jacket", "key", "lamp", "mountain", "net", "ocean",
            "pillow", "quiet", "ring", "star", "train", "unicorn", "violin", "window", "x-ray", "yard",
            "zoo", "airplane", "bridge", "cloud", "drum", "engine", "feather", "glove", "hammer", "ink",
            "jewel", "kangaroo", "ladder", "mirror", "nest", "owl", "pizza", "quilt", "robot", "ship",
            "table", "utensil", "village", "wheel", "xenon", "yacht", "zucchini", "ant", "bread", "cup",
            "desert", "eagle", "fan", "garden", "hill", "idea", "jungle", "kite", "lake", "moon",
            "needle", "orchid", "parrot", "quartz", "rocket", "snake", "tiger", "urchin", "volcano", "water"
        ];

        Random random = new Random();
        var words = new List<string>();
        int currentWordLength = 0;
        int retryLimit = 100; // prevent infinite loop
        int retries = 0;

        while (currentWordLength < maxCharsLength && retries < retryLimit)
        {
            var word = wordPool[random.Next(wordPool.Length)];
            if (currentWordLength + word.Length <= maxCharsLength)
            {
                words.Add(word);
                currentWordLength += word.Length;
                retries = 0; // reset retry counter on success
            }
            else
            {
                retries++;
            }
        }

        return string.Join(" ", words);
    }
    
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private readonly DispatcherTimer _uiTimer;
    private const int MaxLines = 3;
    private int _currentLine = 0;
    private int _currentWordIndex = 0;
    private int _currentCharIndex = 0; 
    

    private TimeSpan _elapsed;
    public TimeSpan Elapsed
    {
        get => _elapsed;
        private set => SetField(ref _elapsed, value);
    }
    

    public void ProcessInput(char c)
    {
        if (!_stopwatch.IsRunning)
        {
            _stopwatch.Start();
            _uiTimer.Start();
        }
        
        if (IsLetterOrSpace(c))
        {
            var currentLine = GameText[_currentLine];
            var currentWord = currentLine.Text[_currentWordIndex];
            var currentChar = currentWord.Word[_currentCharIndex];
            
            if (currentChar.Letter == ' ')
            {
                if (c == ' ')
                {
                    _currentWordIndex++;
                }
            }
            else if (c == currentChar.Letter)
            {
                currentChar.State = CharacterState.Correct;
                _currentCharIndex++;
                if (_currentCharIndex == currentWord.Word.Count)
                {
                    _currentWordIndex++;
                    _currentCharIndex = 0;
                }
            }
            else if (c != currentChar.Letter)
            {
                currentChar.State = CharacterState.Incorrect;
                _currentCharIndex++;
                if (_currentCharIndex == currentWord.Word.Count)
                {
                    _currentWordIndex++;
                    _currentCharIndex = 0;
                }
            }
            
        }
    }

    private bool IsLetterOrSpace(char c)
    {
        bool isValid = char.IsLetter(c) || c == ' ' || c == ':' || c == ',' || c == '.';
        return isValid;
    }
}




