using System.Collections.ObjectModel;
using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class ObservableWord : ObservableObject
{
    private ObservableCollection<ObservableChar> _word = new();
    private bool _isWritten;
    private bool _isCaretAtWordEnd;

    public ObservableWord(string word)
    {
        foreach (char c in word)
        {
            Word.Add(new ObservableChar(c));
        }
    }

    public ObservableCollection<ObservableChar> Word
    {
        get => _word;
        set => SetField(ref _word, value);
    }

    public bool IsWritten
    {
        get => _isWritten;
        set => SetField(ref _isWritten, value);
    }

    public bool IsCaretAtWordEnd
    {
        get => _isCaretAtWordEnd;
        set => SetField(ref _isCaretAtWordEnd, value);
    }
}
