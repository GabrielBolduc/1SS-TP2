using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get => _dialogResult;
            set => Set(ref _dialogResult, value);
        }

        public RelayCommand SauverCommand { get; }
        public RelayCommand AnnulerCommand { get; }

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

                DialogResult = true; // ferme la fenêtre
            });

            AnnulerCommand = new RelayCommand(_ => DialogResult = false);
        }
    }
}