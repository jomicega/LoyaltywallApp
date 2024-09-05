using Loyaltywall.Prism.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Rg.Plugins.Popup.Services;

namespace Loyaltywall.Prism.ViewModels
{
    public class AlertDisplayPageViewModel : BindableBase
    {
        private DelegateCommand _cerrarPopUpCommand;
        private string _message;        

        public AlertDisplayPageViewModel()
        {
            Message = Settings.Error;
            
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        
        public DelegateCommand CerrarPopUpCommand => _cerrarPopUpCommand ?? (_cerrarPopUpCommand = new DelegateCommand(CerrarPopUpAsync));

        private async void CerrarPopUpAsync()
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}