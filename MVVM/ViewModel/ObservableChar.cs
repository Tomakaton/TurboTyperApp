using TurboTyper.Core;
using TurboTyper.MVVM.ViewModel.Enums;

namespace TurboTyper.MVVM.ViewModel;

public class ObservableChar : ObservableObject
{
    private CharacterState _state = CharacterState.NotTyped;
    private bool _isCurrent;

    public ObservableChar(char letter)
    {
        Letter = letter;
    }

    public char Letter { get; }

    public CharacterState State
    {
        get => _state;
        set => SetField(ref _state, value);
    }

    public bool IsCurrent
    {
        get => _isCurrent;
        set => SetField(ref _isCurrent, value);
    }
}
