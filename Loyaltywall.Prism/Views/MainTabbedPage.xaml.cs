using Loyaltywall.Prism.ViewModels;
using System;
using Xamarin.Forms;

namespace Loyaltywall.Prism.Views
{
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            
        }

        private void OnCurrentPageChanged(object sender, EventArgs e)
        {
            if (CurrentPage is PointsPage pointsPage && pointsPage.BindingContext is PointsPageViewModel viewModel)
            {
                viewModel.PropertyUpdate();
            }
        }
    }
}
