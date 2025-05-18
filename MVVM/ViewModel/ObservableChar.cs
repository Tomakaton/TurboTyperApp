using TurboTyper.Core;
using TurboTyper.MVVM.ViewModel.Enums;

namespace TurboTyper.MVVM.ViewModel;

public class ObservableChar : ObservableObject
{
    private char _letter;
    private CharacterState _state = CharacterState.NotTyped;

    public ObservableChar(char letter)
    {
        Letter = letter;
    }

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
}
