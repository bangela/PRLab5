using Acr.UserDialogs;
using MvvmCross.IoC;
using Sharing.Client.Core.Client;
using Sharing.Client.Core.Interfaces;

namespace Sharing.Client.Core
{
    public static class SetupIoC
    {
        public static IMvxIoCProvider RegisterDependencies(IMvxIoCProvider provider)
        {
            provider.ConstructAndRegisterSingleton<IUDPClient, UDPClient>();
            provider.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
            return provider;
        }
    }
}
