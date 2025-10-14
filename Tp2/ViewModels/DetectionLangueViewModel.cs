using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

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
            DetectCommand = new RelayCommand(_ => Detect(), _ => !string.IsNullOrWhiteSpace(InputText));
        }

        private void Detect()
        {
            var token = Properties.Settings.Default.ApiToken;
            if (string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show("Aucun jeton configuré. Ouvrez Configuration et entrez votre API token.",
                                "Détection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Étape 2 : stub d’affichage
            Resultats.Clear();
            Resultats.Add("FRENCH");
            Resultats.Add("ENGLISH");
            SelectedResult = "FRENCH";

            // Étape 3 : appel HTTP réel vers /detect (avec token)
        }
    }
}