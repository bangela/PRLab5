using MvvmCross.Forms.Views;
using Sharing.Client.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Sharing.Client.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstPage : MvxContentPage<FirstPageViewModel>
    {
        public FirstPage()
        {
            InitializeComponent();
        }
    }
}