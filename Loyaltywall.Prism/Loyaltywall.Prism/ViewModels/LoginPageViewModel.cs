using Loyaltywall.Prism.Helpers;
using Loyaltywall.Prism.Models;
using Loyaltywall.Prism.Responses;
using Loyaltywall.Prism.Views;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Collections.Generic;
using Loyaltywall.Prism.Services;
using System.Linq;

namespace Loyaltywall.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public static INavigationService _navigationService;
        private readonly IApiService _apiService;
        OidcClientKeyCloak oidcClientKeyCloak = new OidcClientKeyCloak(true);
        private List<User> _ListPlanUser;
        public LoginPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _ = LoginAsync();
        }

        public async Task LoginAsync()
        {
            var response = await oidcClientKeyCloak.LoginAsync();
            if (!response.IsSuccess == true)
            {
                Settings.TokenExpiresIn = response.TokenExpiresIn;
                Settings.Name = response.Name;
                Settings.Email = response.Email;
                Settings.IdKeycloak = response.Id;
                LoadPointsAsync();
                await _navigationService.NavigateAsync(nameof(MainTabbedPage));
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/LoginPage");
            }

        }

        private async void LoadPointsAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a Internet.", "Accept");
                return;
            }

            UseridKeycloak user = new UseridKeycloak
            {
                idKeycloak = Settings.IdKeycloak

            };

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetPonitsAsync(url, "/puntos-usuario", "/points_movement/point_total", user);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Usuario no contiene información", "Accept");
                return;
            }

            PointsTotalUserResponse Points = (PointsTotalUserResponse)response.Result;
            _ListPlanUser = Points.user;
            Settings.ListPlan = new ObservableCollection<User>(_ListPlanUser.Select(s => new User()
            {
                isActive = s.isActive,
                description = s.description,    
                name = s.name

            }).ToList());
            Settings.Current_points = Points.total_points.ToString();
            

            Response responsePonitsexpired = await _apiService.GetPonitsexpiredAsync(url, "/puntos-usuario", "/points_expired/" + Settings.IdKeycloak);
            if (!responsePonitsexpired.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Usuario no contiene información", "Accept");
                return;
            }

            TotalpointsExpireResponse PointsExpired = (TotalpointsExpireResponse)responsePonitsexpired.Result;
            Settings.Expired_points = PointsExpired.total_points_to_expire.ToString();
            if (PointsExpired.date_to_expire != null && PointsExpired.date_to_expire.Any())
            {
                DateToExpire primerElemento = PointsExpired.date_to_expire.First();
                string expiration_point_date = primerElemento.expiration_date.ToString("d");
                Settings.Expiration_point_date = expiration_point_date.ToString();
                return;
            }
            else
            {
                Settings.Expiration_point_date = "";
            }
            
        }
    }
}
