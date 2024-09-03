using Loyaltywall.Prism.Responses;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Loyaltywall.Prism.Helpers;

namespace Loyaltywall.Prism.ViewModels
{
    public class PlandetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private List<User> _ListPlanUser;
        private ObservableCollection<User> _listPlan;
        private string _name;
        private string _description;

        public PlandetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            ShowList();
            ShowPlanActive();
        }       

        public ObservableCollection<User> ListPlan
        {
            get => _listPlan;
            set => SetProperty(ref _listPlan, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        private void ShowList()
        {
            _listPlan = Settings.ListPlan;
            ListPlan = new ObservableCollection<User>(_listPlan.Select(p => new User()
            {
                isActive = p.isActive, 
                description = p.description,    
                name = p.name

            }).ToList());
        }

        private void ShowPlanActive()
        {
            User userWithActiveStatus = ListPlan.FirstOrDefault(user => user.isActive == true);
            if (userWithActiveStatus != null)
            {
                Name = userWithActiveStatus.name;
                Description = userWithActiveStatus.description; 
            }
        }

        
    }
}
