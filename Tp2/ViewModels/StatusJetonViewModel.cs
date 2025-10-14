using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp2.ViewModels
{
    public sealed class StatusJetonViewModel : BaseViewModel
    {
        private string _status = "Aucun statut chargé.";
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        public AsyncCommand RafraichirCommand { get; }

        public StatusJetonViewModel()
        {
            RafraichirCommand = new AsyncCommand(async () =>
            {
                await System.Threading.Tasks.Task.Delay(100);
                Status = "Plan: FREE\nRequêtes aujourd'hui: 0\nStatut: ACTIVE (stub)";
            });
        }
    }
}

