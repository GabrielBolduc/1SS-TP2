using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Tp2.ViewModels
{
    public sealed class DetectionLangueViewModel : BaseViewModel
    {
        private string _inputText = "";
        public string InputText
        {
            get => _inputText;
            set { if (Set(ref _inputText, value)) DetectCommand.RaiseCanExecuteChanged(); }
        }

        public ObservableCollection<string> Resultats { get; } = new();

        private string? _selected;
        public string? SelectedResult
        {
            get => _selected;
            set => Set(ref _selected, value);
        }

        public RelayCommand DetectCommand { get; }

        public DetectionLangueViewModel()
        {
            DetectCommand = new RelayCommand(_ => DetectStub(), _ => !string.IsNullOrWhiteSpace(InputText));
        }

        private void DetectStub()
        {
            // stub pour étape 1
            Resultats.Clear();
            Resultats.Add("FRENCH");
            Resultats.Add("ENGLISH");
            SelectedResult = "FRENCH";
        }
    }
}

