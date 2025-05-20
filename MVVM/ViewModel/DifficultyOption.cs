using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class DifficultyOption : ObservableObject
{
    private bool _isSelected;
    public int Value { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (SetField(ref _isSelected, value) && value)
            {
                // Notify parent when selected
                Selected?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public event EventHandler? Selected;
}