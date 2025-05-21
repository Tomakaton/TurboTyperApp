using System.Collections.ObjectModel;
using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class ObservableWord : ObservableObject
{
    public ObservableWord(string word)
    {
        SetObservableWord(word);
    }
    
    private ObservableCollection<ObservableChar> _word = new();

    public ObservableCollection<ObservableChar> Word
    {
        get => _word; 
        set => SetField(ref _word, value);
    }
    
    private void SetObservableWord(string word)
    {
        foreach (var c in word)
        {
            Word.Add(new ObservableChar(c));
        }
    }
}