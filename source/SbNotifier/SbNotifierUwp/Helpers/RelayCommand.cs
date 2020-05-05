using System;
using System.Windows.Input;

namespace SbNotifierUwp.Helpers
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => canExecute == null || canExecute(parameter);

        public void Execute(object parameter) => execute?.Invoke(parameter);

        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        private readonly Action<object> execute = null;
        private readonly Predicate<object> canExecute = null;
    }
}