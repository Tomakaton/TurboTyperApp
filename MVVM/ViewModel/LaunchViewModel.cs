using System;
using System.Windows.Input;
using TurboTyper.Core;

namespace TurboTyper.MVVM.ViewModel;

public class LaunchViewModel : ObservableObject
{
    public LaunchViewModel() : this(null)
    {
    }

    public LaunchViewModel(Action? onPlay)
    {
        PlayCommand = new RelayCommand(() => onPlay?.Invoke());
    }

    public ICommand PlayCommand { get; }
}
