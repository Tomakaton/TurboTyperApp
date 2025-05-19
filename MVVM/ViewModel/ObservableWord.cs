using System.Collections.ObjectModel;
using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class ObservableWord : ObservableObject
{
    public ObservableWord(string word)
    {
        SetObserableWord(word);
    }
    
    private ObservableCollection<ObservableChar> _word = new();

    public ObservableCollection<ObservableChar> Word
    {
        get => _word; 
        set => SetField(ref _word, value);
    }
    
    private void SetObserableWord(string word)
    {
        foreach (var c in word)
        {
            Word.Add(new ObservableChar(c));
        }
    }
}