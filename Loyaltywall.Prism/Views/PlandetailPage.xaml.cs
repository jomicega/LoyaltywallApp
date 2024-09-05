using Xamarin.Forms;

namespace Loyaltywall.Prism.Views
{
    public partial class PlandetailPage : ContentPage
    {
        public PlandetailPage()
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#7E2EFF");
        }
    }
}
