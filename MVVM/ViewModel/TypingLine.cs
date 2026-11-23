using System;
using System.Collections.ObjectModel;
using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class TypingLine : ObservableObject
{
    private ObservableCollection<ObservableWord> _text = new();
    private bool _isActive = true;

    public TypingLine(string line)
    {
        SetTypingLine(line);
    }

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
        string[] words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words)
        {
            Text.Add(new ObservableWord(word));
        }
    }
}
