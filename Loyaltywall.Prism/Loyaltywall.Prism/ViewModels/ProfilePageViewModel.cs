using Loyaltywall.Prism.Helpers;
using Loyaltywall.Prism.Responses;
using Loyaltywall.Prism.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Navigation.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Loyaltywall.Prism.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private string _name;
        private string _email;
        private DelegateCommand _planDetailCommand;

        public ProfilePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Name = Settings.Name;
            Email = Settings.Email;
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public DelegateCommand PlanDetailCommand => _planDetailCommand ?? (_planDetailCommand = new DelegateCommand(PlanDetailAsync));        

        private async void PlanDetailAsync()
        {
            await _navigationService.NavigateAsync(nameof(PlandetailPage));
        }

        public ICommand LogoutCommand => new Command(async () => await LogOutAsync());

        public async Task LogOutAsync()
        {

            OidcClientKeyCloak oidcClientKeyCloak = new OidcClientKeyCloak(false);
            bool logoutResult = await oidcClientKeyCloak.LogoutAsync(); 
            await Task.Delay(3000);
            if (logoutResult == true) 
            {
                Settings.TokenExpiresIn = 0;
                Settings.Name = string.Empty;
                Settings.Email = string.Empty;
                Settings.Current_points = string.Empty;
                Settings.Expired_points = string.Empty;
                Settings.Expiration_point_date = string.Empty;  
                Settings.IdKeycloak = string.Empty;
                Settings.ListPlan = new ObservableCollection<User>();
                Application.Current.MainPage = new LoginPage();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "No se pudo cerra la sesión", "Accept");
            }

            
        }
    }
}
