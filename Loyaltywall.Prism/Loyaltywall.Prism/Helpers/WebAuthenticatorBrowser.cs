using IdentityModel.OidcClient.Browser;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.Renderscripts.Script;

namespace Loyaltywall.Prism.Helpers
{
    public class SystemBrowser : IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<BrowserResult>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                var webView = new WebView
                {
                    Source = options.StartUrl,
                };

                webView.Navigated += async (s, e) =>
                {
                    try
                    {
                        if (!tcs.Task.IsCompleted)
                        {
                            if (e.Url.StartsWith(options.EndUrl))
                            {
                                tcs.SetResult(new BrowserResult { Response = e.Url });
                                await Application.Current.MainPage.Navigation.PopModalAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                };

                await Application.Current.MainPage.Navigation.PushModalAsync(new ContentPage
                {
                    Content = webView,
                });
            });

            return await tcs.Task;
        }

        //public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        //{
        //    var tcs = new TaskCompletionSource<BrowserResult>();

        //    try
        //    {
        //        var startUri = new Uri(options.StartUrl);

        //        // Abrir la URL en el navegador del dispositivo
        //        var openResult =  Browser.OpenAsync(startUri, BrowserLaunchMode.SystemPreferred);

        //        // Verificar si se abrió el navegador con éxito
        //        if (openResult.IsCompleted)
        //        {
        //            // Esperar un tiempo razonable para que el usuario interactúe con el navegador
        //            await Task.Delay(10000); // Por ejemplo, espera 10 segundos

        //            // Obtener la URL actual del navegador
        //            var currentUrl = openResult.ToString();

        //            // Comprobar si la URL actual del navegador coincide con la URL de inicio
        //            if (currentUrl == startUri.AbsoluteUri)
        //            {
        //                tcs.SetResult(new BrowserResult { Response = currentUrl });
        //                await Application.Current.MainPage.Navigation.PopModalAsync();
        //            }
        //        }
        //        else
        //        {
        //            // Si el navegador no se abrió o no se completó, manejarlo según sea necesario
        //            tcs.SetResult(new BrowserResult { Response = null });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        tcs.SetException(ex);
        //    }

        //    return await tcs.Task;
        //}
    }

    public class LogoutBrowser : IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<BrowserResult>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                var webView = new WebView
                {
                    Source = options.StartUrl,
                };

                webView.Navigated += (s, e) =>
                {
                    try
                    {
                        if (!tcs.Task.IsCompleted) // Verifica si no está completado
                        {
                            if (e.Url.StartsWith(options.EndUrl))
                            {
                                tcs.SetResult(new BrowserResult { Response = e.Url });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex); // Manejo de excepciones
                    }
                };

                await Application.Current.MainPage.Navigation.PushModalAsync(new ContentPage
                {
                    Content = webView,
                });
            });

            return await tcs.Task;
        }
    }
}


