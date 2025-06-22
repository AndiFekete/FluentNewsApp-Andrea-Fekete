using FluentNewsApp.ViewModels;

namespace FluentNewsApp.Commands
{
    public class RelayCommand : AsyncCommandBase
    {
        private readonly Func<Task> _execute;

        public RelayCommand(Func<Task> execute, Action<Exception> onException) : base(onException)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            await _execute();
        }
    }
}
