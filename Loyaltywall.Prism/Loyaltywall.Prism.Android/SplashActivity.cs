using Android.App;
using Android.Content;
using AndroidX.AppCompat.App;

namespace Loyaltywall.Prism.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash",
              MainLauncher = true,
              Icon = "@drawable/Logo_solo",
              NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
