using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tp2.ViewModels.Commands;

namespace Tp2.ViewModels
{
    public sealed class ConfigurationViewModel : BaseViewModel
    {
        private string _jeton = "";
        public string Jeton
        {
            get => _jeton;
            set => Set(ref _jeton, value);
        }

        public RelayCommand SauverCommand { get; }

        public event EventHandler? RequestClose;

        public ConfigurationViewModel()
        {
            Jeton = Properties.Settings.Default.ApiToken ?? string.Empty;

            SauverCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrWhiteSpace(Jeton))
                {
                    MessageBox.Show("Veuillez entrer un jeton valide.", "Configuration",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Properties.Settings.Default.ApiToken = Jeton.Trim();
                Properties.Settings.Default.Save();

                MessageBox.Show("Jeton sauvegardé.", "Configuration",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                // Demande à la vue de se fermer
                RequestClose?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
