using Loyaltywall.Prism.Helpers;
using Loyaltywall.Prism.Services;
using Loyaltywall.Prism.ViewModels;
using Loyaltywall.Prism.Views;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Syncfusion.Licensing;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace Loyaltywall.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MjY3OTgwMUAzMjMyMmUzMDJlMzBoNUEvRzVqdGpobmp2Qm1USjdRa0Mwa2hGRS9RLytoN2hPc1JITGYwdGtrPQ==");
            InitializeComponent();
            if (Settings.TokenExpiresIn > 0)
            {
                await NavigationService.NavigateAsync("NavigationPage/MainTabbedPage");
                return;
            }
            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainTabbedPage, MainTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<PointsPage, PointsPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<PlandetailPage, PlandetailPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<AlertDisplayPage, AlertDisplayPageViewModel>();
            containerRegistry.Register<IApiService, ApiServices>();
        }
    }
}
