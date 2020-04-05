using MvvmCross.Forms.Platforms.Uap.Core;
using MvvmCross.IoC;
using Sharing.Client.Core;

namespace Sharing.Client.UWP
{
    public class Setup : MvxFormsWindowsSetup<Sharing.Client.Core.App, Sharing.Client.App>
    {
        protected override IMvxIoCProvider InitializeIoC()
        {
            var provider = base.InitializeIoC();
            return SetupIoC.RegisterDependencies(provider);
        }
    }
}
