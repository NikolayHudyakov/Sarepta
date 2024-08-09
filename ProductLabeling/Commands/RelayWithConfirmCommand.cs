using ProductLabeling.Views.Windows;
using System;
using System.Threading.Tasks;

namespace ProductLabeling.Commands
{
    internal class RelayWithConfirmCommand : Command
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayWithConfirmCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayWithConfirmCommand(Action execute, Predicate<object?>? canExecute = null)
        {
            _execute = (parameter) => execute();
            _canExecute = canExecute;
        }

        public RelayWithConfirmCommand(Func<Task> execute, Predicate<object?>? canExecute = null)
        {
            _execute = (parameter) => execute();
            _canExecute = canExecute;
        }

        public RelayWithConfirmCommand(Func<object?, Task> execute, Predicate<object?>? canExecute = null)
        {
            _execute = (parameter) => execute(parameter);
            _canExecute = canExecute;
        }

        public override void Execute(object? parameter)
        {
            if(new ActionConfirmationWindow().ShowDialog() ?? false)
                _execute(parameter);
        }

        public override bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
    }
}
