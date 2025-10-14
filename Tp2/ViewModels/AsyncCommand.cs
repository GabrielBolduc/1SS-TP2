using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tp2.ViewModels
{
    public sealed class AsyncCommand : ICommand
    {
        private readonly Func<Task> _exec;
        private readonly Func<bool>? _can;
        private bool _running;

        public AsyncCommand(Func<Task> exec, Func<bool>? can = null)
        {
            _exec = exec ?? throw new ArgumentNullException(nameof(exec));
            _can = can;
        }

        public bool CanExecute(object? parameter) => !_running && (_can?.Invoke() ?? true);
        public async void Execute(object? parameter)
        {
            if (!CanExecute(null)) return;
            try { _running = true; CanChanged(); await _exec(); }
            finally { _running = false; CanChanged(); }
        }
        public event EventHandler? CanExecuteChanged;
        public void CanChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
