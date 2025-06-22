using System.Windows.Input;

namespace FluentNewsApp.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {
        private bool _isExecuting;
        private readonly Action<Exception> _onException;

        public bool IsExecuting
        {
            get => _isExecuting;
            protected set
            {
                if (_isExecuting != value)
                {
                    _isExecuting = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler? CanExecuteChanged;

        public AsyncCommandBase(Action<Exception> onException)
        {
            _onException = onException;
        }

        public virtual bool CanExecute(object? parameter)
        {
            return !IsExecuting;
        }

        public async void Execute(object? parameter)
        {
            IsExecuting = true;

            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }

            IsExecuting = false;
        }

        protected abstract Task ExecuteAsync(object? parameter);
    }
}
