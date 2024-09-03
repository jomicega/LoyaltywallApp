using Loyaltywall.Prism.Helpers;
using Loyaltywall.Prism.Models;
using Loyaltywall.Prism.Responses;
using Loyaltywall.Prism.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Loyaltywall.Prism.ViewModels
{
    public class PointsPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        string _BTNbackground = "#F95F62";
        private DateTime _selectedDate = DateTime.Now;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _current_points;
        private string _expired_points;
        private string _name;
        private List<UserEvent> _purchased_history_points;
        private List<UserConsumed> _consumed_history_points;
        private ObservableCollection<UserEvent> _purchased;
        private DelegateCommand _acomuladosCommand;
        private DelegateCommand _consumidosCommand;
        private ObservableCollection<UserConsumed> _consumed;
        private ObservableCollection<TotalMovementPoint> _all;
        private bool _isVisibleListPurchased;
        private bool _isVisibleListConsumed;
        private bool _isVisibleListAll;
        private bool _isRunning;
        private DelegateCommand _todosCommand;
        private List<TotalMovementPoint> _all_history_points;
        private bool _isCheckedTodos;
        private bool _isCheckedAcomulados;
        private bool _isCheckedConsumidos;
        private string _eventName;

        public PointsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Name = Settings.Name; 
            BTNbackground = "#F95F62";            
            TodosCommandAsync();
            IsVisibleListPurchased = false;
            IsVisibleListConsumed = false;
            IsVisibleListAll = false;
            IsCheckedTodos = true;

        }       

        public void PropertyUpdate()
        {
            Current_points = Settings.Current_points;
            Expired_points = Settings.Expired_points;
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string EventName
        {
            get => _eventName;
            set => SetProperty(ref _eventName, value);
        }

        public string Current_points
        {
            get => _current_points;
            set => SetProperty(ref _current_points, value);
        }
        public string Expired_points
        {
            get => _expired_points;
            set => SetProperty(ref _expired_points, value);
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    SetProperty(ref _selectedDate, value);
                    FiltrarRegistrosPorFecha();

                }
            }
        }


        public bool IsVisibleListPurchased
        {
            get => _isVisibleListPurchased;
            set
            {
                SetProperty(ref _isVisibleListPurchased, value);

            }
        }

        public string BTNbackground
        {
            get
            {
                return _BTNbackground;
            }

            set
            {
                if (_BTNbackground != value)
                {
                    _BTNbackground = value;
                    SetProperty(ref _BTNbackground, value);

                }
            }

        }
        public ObservableCollection<TotalMovementPoint> All
        {
            get { return _all; }
            set
            {
                SetProperty(ref _all, value);
            }
        }


        public bool IsCheckedTodos
        {
            get => _isCheckedTodos;
            set => SetProperty(ref _isCheckedTodos, value);
        }

        public bool IsCheckedAcomulados
        {
            get => _isCheckedAcomulados;
            set => SetProperty(ref _isCheckedAcomulados, value);
        }

        public bool IsCheckedConsumidos
        {
            get => _isCheckedConsumidos;
            set => SetProperty(ref _isCheckedConsumidos, value);
        }

        public bool IsVisibleListConsumed
        {
            get => _isVisibleListConsumed;
            set => SetProperty(ref _isVisibleListConsumed, value);
        }

        public bool IsVisibleListAll
        {
            get => _isVisibleListAll;
            set => SetProperty(ref _isVisibleListAll, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public ObservableCollection<UserEvent> Purchased
        {
            get => _purchased;
            set => SetProperty(ref _purchased, value);
        }

        public ObservableCollection<UserConsumed> Consumed
        {
            get => _consumed;
            set => SetProperty(ref _consumed, value);
        }


        public DelegateCommand AcomuladosCommand => _acomuladosCommand ?? (_acomuladosCommand = new DelegateCommand(AcomuladosCommandAsync));

        public DelegateCommand ConsumidosCommand => _consumidosCommand ?? (_consumidosCommand = new DelegateCommand(ConsumidosCommandAsync));

        public DelegateCommand TodosCommand => _todosCommand ?? (_todosCommand = new DelegateCommand(TodosCommandAsync));

        private async void TodosCommandAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a Internet.", "Accept");
                return;
            }

            
            IsCheckedAcomulados = false;
            IsCheckedConsumidos = false;
            IsCheckedTodos = true;
            IsVisibleListPurchased = false;
            IsVisibleListConsumed = false;
            IsVisibleListAll = false;
            IsRunning = true;


            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAllPointsAsync(url, "/puntos-usuario", "/puntos_movement/list/" + Settings.IdKeycloak);
            await Task.Delay(1000);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }


            PointAllUser Points = (PointAllUser)response.Result;
            _all_history_points = Points.total_movement_points;
            SelectedDate = DateTime.Now;
            ShowPointsAll();
            return;
        }

        private async void ConsumidosCommandAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a Internet.", "Accept");
                return;
            }

            
            IsCheckedAcomulados = false;
            IsCheckedConsumidos = true;
            IsCheckedTodos = false;
            IsVisibleListPurchased = false;
            IsVisibleListConsumed = false;
            IsVisibleListAll = false;
            IsRunning = true;

            UseridKeycloak user = new UseridKeycloak
            {
                idKeycloak = Settings.IdKeycloak
            };
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListconsumedPonitsAsync(url, "/puntos-usuario", "/points_movement", user);
            await Task.Delay(1000);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            PointConsumedUser Points = (PointConsumedUser)response.Result;
            _consumed_history_points = Points.user_consumed;
            SelectedDate = DateTime.Now;
            ShowPointsConsumed();
            return;
        }

        private async void AcomuladosCommandAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a Internet.", "Accept");
                return;
            }
            
            IsCheckedAcomulados = true; 
            IsCheckedConsumidos = false; 
            IsCheckedTodos = false;
            IsVisibleListPurchased = false;
            IsVisibleListConsumed = false;
            IsVisibleListAll = false;
            IsRunning = true;

            UseridKeycloak user = new UseridKeycloak
            {
                idKeycloak = Settings.IdKeycloak

            };
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAccumulatedPonitsAsync(url, "/puntos-usuario", "/puntos_events", user);
            IsRunning = false;


            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }


            PointEventsUser pointsEvents = (PointEventsUser)response.Result;
            _purchased_history_points = pointsEvents.user_events;
            SelectedDate = DateTime.Now;
            ShowPointspurchased();


        }

        private void ShowPointsAll()
        {
            IsVisibleListPurchased = false;
            IsVisibleListAll = true;
            IsVisibleListConsumed = false;

            All = new ObservableCollection<TotalMovementPoint>(_all_history_points.Select(p => new TotalMovementPoint()
            {
                name = p.name,
                points = p.points,
                registration_date = p.registration_date

            }).ToList());
            return;

        }

        private void ShowPointsConsumed()                                   
        {
            IsVisibleListPurchased = false;
            IsVisibleListAll = false;
            IsVisibleListConsumed = true;
            Consumed = new ObservableCollection<UserConsumed>(_consumed_history_points.Select(p => new UserConsumed()
            {
                points = p.points,
                system_date = p.system_date,
                @event = new EventConsumed { name = p.@event.name }

            }).ToList());
        }

        private void ShowPointspurchased()
        {
            IsVisibleListPurchased = true;
            IsVisibleListConsumed = false;
            IsVisibleListAll = false;
            Purchased = new ObservableCollection<UserEvent>(_purchased_history_points.Select(p => new UserEvent()
            {
                points = p.points,
                registration_date = p.registration_date,  
                @event =p.@event

            }).ToList());
        }

        private void FiltrarRegistrosPorFecha()
        {
            if (SelectedDate != DateTime.MinValue)
            {
                var filtroPointsAll = _all_history_points;
                var registrosFiltrados = new ObservableCollection<TotalMovementPoint>(
                    filtroPointsAll.Where(r => r.registration_date.Year == SelectedDate.Date.Year &&
                                   r.registration_date.Month == SelectedDate.Date.Month &&
                                   r.registration_date.Day == SelectedDate.Date.Day)
                );
                var FiltroPointsAllObservableCollection = registrosFiltrados.ToList<TotalMovementPoint>();
                IsVisibleListPurchased = false;
                IsVisibleListAll = true;
                IsVisibleListConsumed = false;
                All = new ObservableCollection<TotalMovementPoint>(FiltroPointsAllObservableCollection.Select(p => new TotalMovementPoint()
                {
                    name = p.name,
                    points = p.points,
                    registration_date = p.registration_date

                }).ToList());

            }
            else
            {
                All = new ObservableCollection<TotalMovementPoint>(_all_history_points.Select(p => new TotalMovementPoint()
                {
                    name = p.name,
                    points = p.points,
                    registration_date = p.registration_date

                }).ToList());
            }
            return;
        }


    }

}





