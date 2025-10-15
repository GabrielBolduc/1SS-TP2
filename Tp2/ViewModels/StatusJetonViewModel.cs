using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tp2.Models;
using Tp2.ViewModels.Commands;

namespace Tp2.ViewModels
{
    public sealed class StatusJetonViewModel : BaseViewModel
    {
        private readonly DetectLanguageService _service = new();

        private string _status = "Aucun statut chargé.";
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        public AsyncCommand RafraichirCommand { get; }
        public RelayCommand FermerCommand { get; } // si tu as gardé la fermeture via VM

        public event EventHandler? RequestClose;

        public StatusJetonViewModel()
        {
            RafraichirCommand = new AsyncCommand(LoadAsync);
            FermerCommand = new RelayCommand(_ => RequestClose?.Invoke(this, EventArgs.Empty));
        }

        private async Task LoadAsync()
        {
            var token = Properties.Settings.Default.ApiToken;
            if (string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show("Aucun jeton configuré. Ouvrez Configuration et entrez votre API token.",
                                "Statut du jeton", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var st = await _service.GetStatusAsync();
                var sb = new StringBuilder();
                sb.AppendLine($"Plan: {st.plan}");
                sb.AppendLine($"Statut: {st.status}");
                sb.AppendLine($"Date: {st.date}");
                sb.AppendLine($"Requêtes aujourd'hui: {st.requests_today} / {st.daily_requests_limit}");
                sb.AppendLine($"Octets aujourd'hui: {st.bytes_today} / {st.daily_bytes_limit}");
                if (!string.IsNullOrWhiteSpace(st.plan_expires))
                    sb.AppendLine($"Expiration du plan: {st.plan_expires}");
                Status = sb.ToString();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erreur HTTP statut jeton:\n{ex.Message}",
                                "Statut du jeton", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur statut jeton:\n{ex.Message}",
                                "Statut du jeton", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
