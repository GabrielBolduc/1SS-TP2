using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tp2.Models;
using Tp2.ViewModels.Commands;

namespace Tp2.ViewModels
{
    public sealed class DetectionLangueViewModel : BaseViewModel
    {
        private readonly DetectLanguageService _service = new();

        // Texte multiligne saisi
        private string _inputText = "";
        public string InputText
        {
            get => _inputText;
            set
            {
                if (Set(ref _inputText, value))
                    DetectCommand?.RaiseCanExecuteChanged();
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (Set(ref _isBusy, value))
                    DetectCommand?.RaiseCanExecuteChanged();
            }
        }

        private string _busyText = "";
        public string BusyText
        {
            get => _busyText;
            set => Set(ref _busyText, value);
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
            DetectCommand = new AsyncCommand(DetectAsync, CanDetect);
        }

        private bool CanDetect() => !IsBusy && !string.IsNullOrWhiteSpace(InputText);

        private async Task DetectAsync()
        {
            var token = Properties.Settings.Default.ApiToken;
            if (string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show("Aucun jeton configuré. Ouvrez Configuration et entrez votre API token.",
                                "Détection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var bytes = Encoding.UTF8.GetByteCount(InputText);
            const int softLimit = 256 * 1024;
            if (bytes > softLimit)
            {
                var go = MessageBox.Show(
                    $"Votre texte fait ~{bytes / 1024} Ko. " +
                    "Le plan gratuit limite à ~1 Mo/jour. Continuer ?",
                    "Texte volumineux",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (go != MessageBoxResult.Yes) return;
            }

            try
            {
                IsBusy = true;
                BusyText = "Détection en cours…";

                Resultats.Clear();
                SelectedResult = null;

                var list = await _service.DetectAsync(InputText);

                foreach (var item in list)
                    Resultats.Add(item);

                SelectedResult = Resultats.FirstOrDefault();

                if (Resultats.Count == 0)
                    MessageBox.Show("Aucune langue n’a été détectée.", "Détection",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (HttpRequestException ex)
            {
                var msg = ex.Message;
                if (msg.Contains("401") || msg.Contains("Unauthorized"))
                    MessageBox.Show("Jeton invalide ou manquant (401 Unauthorized). Vérifiez la configuration.",
                                    "Détection", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (msg.Contains("429"))
                    MessageBox.Show("Limite atteinte (429). Réessayez plus tard ou réduisez la taille du texte.",
                                    "Détection", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                    MessageBox.Show($"Erreur HTTP :\n{msg}",
                                    "Détection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la détection :\n{ex.Message}",
                                "Détection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
                BusyText = "";
            }
        }
    }
}
