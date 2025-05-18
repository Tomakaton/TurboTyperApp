using System.Collections.ObjectModel;
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

    public void SetGameText(int lines)
    {
        GameText.Clear();
        for (int i = 0; i < lines; i++)
        {
            GameText.Add(new TypingLine(GetRandomLine(58)));
        }
    }
    
    public static string GetRandomLine(int length, int maxCharsLength = 58)
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
        while (words.Sum(word => word.Length) < maxCharsLength)
        {
            var word = wordPool[random.Next(wordPool.Length)];
            if (words.Sum(s => s.Length) + word.Length <= maxCharsLength)
            {
                words.Add(word);
            }
        }
        return string.Join(" ", words);
    }
}


