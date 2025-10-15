using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tp2.Services;
using Tp2.ViewModels.Commands;

namespace Tp2.ViewModels
{
    public sealed class MainViewModel : BaseViewModel
    {
        public DetectionLangueViewModel DetectionVM { get; }

        public RelayCommand OpenConfigWindowCmd { get; }
        public RelayCommand OpenStatusWindowCmd { get; }

        private readonly IWindowService _windows;

        // VMs des pop-ups
        private readonly ConfigurationViewModel _configVM;
        private readonly StatusJetonViewModel _statusVM;

        public MainViewModel()
            : this(new WindowService()) { }

        public MainViewModel(IWindowService windows)
        {
            _windows = windows;

            DetectionVM = new DetectionLangueViewModel();
            _configVM = new ConfigurationViewModel();
            _statusVM = new StatusJetonViewModel();

            OpenConfigWindowCmd = new RelayCommand(_ => _windows.ShowConfigDialog(_configVM));
            OpenStatusWindowCmd = new RelayCommand(_ => _windows.ShowStatusDialog(_statusVM));
        }
    }
}
