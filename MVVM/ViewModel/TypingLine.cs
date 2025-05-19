using System.Collections.ObjectModel;
using System.Security.AccessControl;
using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class TypingLine : ObservableObject
{
    public TypingLine(string line)
    {
        SetTypingLine(line);
    }
    
    private ObservableCollection<ObservableWord> _text = new();
    private bool _isActive = true;

    public ObservableCollection<ObservableWord> Text
    {
        get => _text; 
        set => SetField(ref _text, value);
    }
    
    public bool IsActive
    {
        get => _isActive;
        set => SetField(ref _isActive, value);
    }
    
    private void SetTypingLine(string line)
    {
        foreach (var word in line.Split(" "))
        {
            Text.Add(new ObservableWord(word));
        }
    }
}