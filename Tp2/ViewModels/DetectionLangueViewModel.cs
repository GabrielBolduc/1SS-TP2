using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Tp2.Models;
using Tp2.ViewModels.Commands;

namespace Tp2.ViewModels
{
    public sealed class DetectionLangueViewModel : BaseViewModel
    {
        private readonly DetectLanguageService _service = new();

        private string _inputText = "";
        public string InputText
        {
            get => _inputText;
            set
            {
                if (Set(ref _inputText, value)) DetectCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<DetectionCandidate> Resultats { get; } = new();

        private DetectionCandidate? _selected;
        public DetectionCandidate? SelectedResult
        {
            get => _selected;
            set => Set(ref _selected, value);
        }

        public AsyncCommand DetectCommand { get; }

        public DetectionLangueViewModel()
        {
            DetectCommand = new AsyncCommand(DetectAsync, () => !string.IsNullOrWhiteSpace(InputText));
        }

        private async Task DetectAsync()
        {
            // 3.2 Règle du sujet: si pas de token -> erreur
            var token = Properties.Settings.Default.ApiToken;
            if (string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show("Aucun jeton configuré. Ouvrez Configuration et entrez votre API token.",
                                "Détection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Resultats.Clear();
                var list = await _service.DetectAsync(InputText.Trim());
                foreach (var item in list) Resultats.Add(item);
                SelectedResult = Resultats.Count > 0 ? Resultats[0] : null;
            }
            catch (HttpRequestException ex)
            {
                // 401: token invalide, 429: limites, 4xx/5xx: autres
                MessageBox.Show($"Erreur HTTP lors de la détection:\n{ex.Message}",
                                "Détection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur lors de la détection:\n{ex.Message}",
                                "Détection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
