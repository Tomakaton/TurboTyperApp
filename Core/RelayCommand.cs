using System;
using System.Windows.Input;

namespace TurboTyper.Core;

/// <summary>
/// A reusable <see cref="ICommand"/> implementation backed by delegates.
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;

    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        ArgumentNullException.ThrowIfNull(execute);
        _execute = _ => execute();
        _canExecute = canExecute is null ? null : _ => canExecute();
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute is null || _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>
    /// Requests WPF's <see cref="CommandManager"/> to re-evaluate <see cref="CanExecute"/>.
    /// </summary>
    public static void RaiseCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}

/// <summary>
/// A strongly-typed <see cref="ICommand"/> implementation backed by delegates.
/// </summary>
public class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _execute;
    private readonly Predicate<T?>? _canExecute;

    public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        if (_canExecute is null) return true;
        if (parameter is null && default(T) is null) return _canExecute(default);
        return parameter is T typed ? _canExecute(typed) : false;
    }

    public void Execute(object? parameter)
    {
        if (parameter is null && default(T) is null)
        {
            _execute(default);
        }
        else if (parameter is T typed)
        {
            _execute(typed);
        }
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public static void RaiseCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}
