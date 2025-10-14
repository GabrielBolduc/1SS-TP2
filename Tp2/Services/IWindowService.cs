using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tp2.Services
{
    public interface IWindowService
    {
        void ShowConfigDialog(object viewModel);
        void ShowStatusDialog(object viewModel);
    }
}
