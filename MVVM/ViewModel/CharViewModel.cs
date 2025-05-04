using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class CharViewModel : ObservableObject
{
    private char _letter;
    private CharacterState _state;

    public char Letter
    {
        get => _letter;
        set => SetField(ref _letter, value);
    }

    public CharacterState State
    {
        get => _state;
        set => SetField(ref _state , value);
    }
    
    public enum CharacterState
    {
        NotTyped,
        Correct,
        Incorrect
    }
}
