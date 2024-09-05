using Loyaltywall.Prism.Helpers;
using Loyaltywall.Prism.Models;
using Loyaltywall.Prism.Responses;
using Loyaltywall.Prism.Services;
using Loyaltywall.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Loyaltywall.Prism.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private string _current_points;
        private List<TotalMovementPoint> _pointAllUser;
        private string _expired_points;
        private string _expiration_point_date;
        private DelegateCommand _perfilCommand;
        private List<User> _ListPlanUser;
        private static HomePageViewModel _instance;
        private DelegateCommand _redeemPointsCommand;
        private ObservableCollection<TotalMovementPoint> _purchased;

        public HomePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _apiService = apiService;
            _navigationService = navigationService;
            Current_points = Settings.Current_points;
            Expired_points = Settings.Expired_points;
            LoadPointsAsync();
            AcomuladosCommandAsync();
        }
        public string Current_points
        {
            get => _current_points;
            set => SetProperty(ref _current_points, value);
        }

        public ObservableCollection<TotalMovementPoint> Purchased
        {
            get => _purchased;
            set => SetProperty(ref _purchased, value);
        }

        public string Expired_points
        {
            get => _expired_points;
            set => SetProperty(ref _expired_points, value);
        }

        public string Expiration_point_date
        {
            get => _expiration_point_date;
            set => SetProperty(ref _expiration_point_date, value);
        }

        public static HomePageViewModel GetInstance()
        {
            return _instance;
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
                name = s.name,
                description = s.description,

            }).ToList());
            Settings.Current_points = Points.total_points.ToString();
            Current_points = Settings.Current_points;

            Response responsePonitsexpired = await _apiService.GetPonitsexpiredAsync(url, "/puntos-usuario", "/points_expired/" + Settings.IdKeycloak);
            if (!responsePonitsexpired.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Usuario no contiene información", "Accept");
                return;
            }

            TotalpointsExpireResponse PointsExpired = (TotalpointsExpireResponse)responsePonitsexpired.Result;
            Settings.Expired_points = PointsExpired.total_points_to_expire.ToString();
            Expired_points = Settings.Expired_points;
            if (PointsExpired.date_to_expire != null && PointsExpired.date_to_expire.Any())
            {
                DateToExpire primerElemento = PointsExpired.date_to_expire.First();
                string expiration_point_date = primerElemento.expiration_date.ToString("d");
                Settings.Expiration_point_date = expiration_point_date.ToString();
                Expiration_point_date = Settings.Expiration_point_date;
                return;
            }
            else
            {
                return;
            }
            
        }

        public ICommand ChipCommand => new Command(async () => await ChipAsync());

        public async Task ChipAsync()
        {
            if (true)
            {

            }
        }


        public DelegateCommand PerfilCommand => _perfilCommand ?? (_perfilCommand = new DelegateCommand(PerfilAsync));

        private async void PerfilAsync()
        {
            var segundaPagina = new ProfilePage();
            await _navigationService.NavigateAsync(nameof(segundaPagina));
        }

        public DelegateCommand RedeemPointsCommand => _redeemPointsCommand ?? (_redeemPointsCommand = new DelegateCommand(RedeemPointsAsync));

        private async void RedeemPointsAsync()
        {
            string googleUrl = App.Current.Resources["googleUrl"].ToString();
            await Browser.OpenAsync(new Uri(googleUrl), BrowserLaunchMode.SystemPreferred);
        }

        private async void AcomuladosCommandAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a Internet.", "Accept");
                return;
            }

            UseridKeycloak user = new UseridKeycloak
            {
                idKeycloak = Settings.Email

            };
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAllPointsAsync(url, "/puntos-usuario", "/puntos_movement/list/" + Settings.IdKeycloak);
          

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Usuario no contiene información", "Accept");
                return;
            }


            PointAllUser pointsEvents = (PointAllUser)response.Result;
            _pointAllUser = pointsEvents.total_movement_points;
            ShowPointspurchased();


        }

        private void ShowPointspurchased()
        {           
            Purchased = new ObservableCollection<TotalMovementPoint>(_pointAllUser.Select(p => new TotalMovementPoint()
            {
                points = p.points,
                registration_date = p.registration_date,
                name = p.name, expiration_date = p.expiration_date

            }).Reverse().Take(5).ToList());
        }
    }
}
