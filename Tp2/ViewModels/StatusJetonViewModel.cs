using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Tp2.Models;
using Tp2.ViewModels.Commands;

namespace Tp2.ViewModels
{
    public sealed class StatusJetonViewModel : BaseViewModel
    {
        private readonly DetectLanguageService _service = new();

        private string _date = "";
        public string Date { get => _date; set => Set(ref _date, value); }

        private int _requestsToday;
        public int RequestsToday { get => _requestsToday; set => Set(ref _requestsToday, value); }

        private int _bytesToday;
        public int BytesToday { get => _bytesToday; set => Set(ref _bytesToday, value); }

        private string _plan = "";
        public string Plan { get => _plan; set => Set(ref _plan, value); }

        private string _planExpires = "";
        public string PlanExpires { get => _planExpires; set => Set(ref _planExpires, value); }

        private int _dailyRequestsLimit;
        public int DailyRequestsLimit { get => _dailyRequestsLimit; set => Set(ref _dailyRequestsLimit, value); }

        private int _dailyBytesLimit;
        public int DailyBytesLimit { get => _dailyBytesLimit; set => Set(ref _dailyBytesLimit, value); }

        private string _status = "";
        public string Status { get => _status; set => Set(ref _status, value); }

        public AsyncCommand RafraichirCommand { get; }
        public RelayCommand FermerCommand { get; }
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

                Date = st.date;
                RequestsToday = st.requests_today;
                BytesToday = st.bytes_today;
                Plan = st.plan;
                PlanExpires = string.IsNullOrWhiteSpace(st.plan_expires) ? "" : st.plan_expires;
                DailyRequestsLimit = st.daily_requests_limit;
                DailyBytesLimit = st.daily_bytes_limit;
                Status = st.status;
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
