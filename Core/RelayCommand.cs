using System.Windows.Input;

namespace TurboTyper.Core;

public class RelayCommand : ICommand
{
    private ICommand _commandImplementation;
    public bool CanExecute(object? parameter)
    {
        return _commandImplementation.CanExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        _commandImplementation.Execute(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => _commandImplementation.CanExecuteChanged += value;
        remove => _commandImplementation.CanExecuteChanged -= value;
    }
}