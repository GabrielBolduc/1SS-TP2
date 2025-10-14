using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ConfigurationViewModel()
        {
            // Étape suivante: sauvegarde dans Properties.Settings + fermer la fenêtre
            SauverCommand = new RelayCommand(_ =>
            {
                System.Windows.MessageBox.Show($"Jeton sauvegardé (stub) : {Jeton}");
            });
        }
    }
}