using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Material_Notepad
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class StandardCommand : ICommand
    {
        public virtual event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter) => canExceute != null ? canExceute(parameter) : true;

        public virtual void Execute(object parameter) => execute?.Invoke(parameter);

        protected Action<object> execute = null;
        protected Func<object, bool> canExceute = null;

        public StandardCommand(Action<object> execute, Func<object, bool> canExceute)
        {
            this.execute = execute;
            this.canExceute = canExceute;
        }

        public StandardCommand(Action<object> execute) : this(execute, null)
        {
        }
    }

    public class RelayCommand : StandardCommand
    {
        public override event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute) : base(execute)
        {
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExceute) : base(execute, canExceute)
        {
        }
    }
}
