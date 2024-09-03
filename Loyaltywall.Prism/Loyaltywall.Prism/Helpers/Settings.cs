using Loyaltywall.Prism.Responses;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Collections.ObjectModel;

namespace Loyaltywall.Prism.Helpers
{
    public static class Settings
    {
        private const string _error = "error";
        private static readonly string _stringDefault = string.Empty;
        private static readonly int _intDefault = 0;
        private static string _tokenExpiresIn = "tokenExpiresIn";
        private static string _expiration_point_date = "expiration_point_date";
        private const string _expired_points = "expired_points";
        private const string _current_points = "current_points";
        private const string _email = "email";
        private const string _idKeycloak = "idKeycloak";
        private const string _name = "nombre";
        private const string _userPlan = "userplan";

        private static ISettings AppSettings => CrossSettings.Current;

        public static string Error
        {
            get => AppSettings.GetValueOrDefault(_error, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_error, value);
        }

        public static string Name
        {
            get => AppSettings.GetValueOrDefault(_name, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_name, value);
        }        
        public static string Email
        {
            get => AppSettings.GetValueOrDefault(_email, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_email, value);
        }

        public static string IdKeycloak
        {
            get => AppSettings.GetValueOrDefault(_idKeycloak, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_idKeycloak, value);
        }
        public static int TokenExpiresIn
        {
            get => AppSettings.GetValueOrDefault(_tokenExpiresIn, _intDefault);
            set => AppSettings.AddOrUpdateValue(_tokenExpiresIn, value);
        }
        public static string Current_points
        {
            get => AppSettings.GetValueOrDefault(_current_points, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_current_points, value);
        }

        public static string Expired_points
        {
            get => AppSettings.GetValueOrDefault(_expired_points, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_expired_points, value);
        }

        public static string Expiration_point_date
        {
            get => AppSettings.GetValueOrDefault(_expiration_point_date, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_expiration_point_date, value);
        }

        public static ObservableCollection<User> ListPlan
        {
            get
            {
                string value = AppSettings.GetValueOrDefault(_userPlan, string.Empty);
                ObservableCollection<User> myList;
                if (string.IsNullOrEmpty(value))
                    myList = new ObservableCollection<User>();
                else
                    myList = JsonConvert.DeserializeObject<ObservableCollection<User>>(value);
                return myList;
            }
            set
            {
                string listValue = JsonConvert.SerializeObject(value);
                AppSettings.AddOrUpdateValue(_userPlan, listValue);
            }
        }

    }
}
