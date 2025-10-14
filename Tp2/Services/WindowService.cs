using System.Windows;
using Tp2.Views;

namespace Tp2.Services
{
    public sealed class WindowService : IWindowService
    {
        public void ShowConfigDialog(object viewModel)
        {
            var win = new ConfigurationWindow
            {
                Owner = Application.Current.MainWindow,
                DataContext = viewModel
            };
            win.ShowDialog();
        }

        public void ShowStatusDialog(object viewModel)
        {
            var win = new StatusJetonWindow
            {
                Owner = Application.Current.MainWindow,
                DataContext = viewModel
            };
            win.ShowDialog();
        }
    }
}
