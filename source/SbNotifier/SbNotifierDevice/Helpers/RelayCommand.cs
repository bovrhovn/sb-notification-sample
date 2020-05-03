using System;
using System.Windows.Input;

namespace SbNotifierDevice.Helpers
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        private readonly Action methodToExecute;
        private readonly Func<bool> canExecuteEvaluator;
        public RelayCommand(Action methodToExecute, Func<bool> canExecuteEvaluator = null)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteEvaluator == null)
            {
                return true;
            }

            bool result = canExecuteEvaluator.Invoke();
            return result;
        }
        public void Execute(object parameter) => methodToExecute.Invoke();
    }
}