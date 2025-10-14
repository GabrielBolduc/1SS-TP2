using System;
using System.Windows;
using Tp2.ViewModels;

namespace Tp2.Views
{
    public partial class ConfigurationWindow : Window
    {
        public ConfigurationWindow()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object? sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ConfigurationViewModel oldVm)
                oldVm.RequestClose -= OnVmRequestClose;

            if (e.NewValue is ConfigurationViewModel newVm)
                newVm.RequestClose += OnVmRequestClose;
        }

        private void OnVmRequestClose(object? sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
