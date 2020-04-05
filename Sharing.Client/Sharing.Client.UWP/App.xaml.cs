using Acr.UserDialogs;
using MvvmCross.Forms.Platforms.Uap.Views;
using Windows.ApplicationModel.Activation;

namespace Sharing.Client.UWP
{
    public abstract class UwpApp : MvxWindowsApplication<Setup, Sharing.Client.Core.App, Sharing.Client.App, MainPage>
    {
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            global::Xamarin.Forms.Forms.Init(e);
            UserDialogs.Init();
            base.OnLaunched(e);
        }
    }
}
