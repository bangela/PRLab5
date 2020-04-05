using Acr.UserDialogs;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Sharing.Client.Core.Interfaces;
using Sharing.Shared;
using System.IO;
using System.Threading;
using Xamarin.Forms;

namespace Sharing.Client.Core.ViewModels
{
    public class FirstPageViewModel : MvxNavigationViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IUDPClient _client;
        private readonly IMvxMainThreadAsyncDispatcher _dispatcher;
        private Thread receiveThread;
        public FirstPageViewModel(IMvxLogProvider provider, IMvxNavigationService navigationService, IUserDialogs userDialogs,
            IUDPClient client, IMvxMainThreadAsyncDispatcher dispatcher)
           : base(provider, navigationService)
        {
            _userDialogs = userDialogs;
            _dispatcher = dispatcher;
            _client = client;
            _client.Initialize("192.168.100.4", 9050);
        }

        private ImageSource _data;
        public ImageSource Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        private IMvxCommand _toggleIsRunningCommand;
        public IMvxCommand ToggleIsRunningCommand => _toggleIsRunningCommand ?? (_toggleIsRunningCommand = new MvxCommand(ToggleIsRunning));

        private void Receive()
        {
            while (IsRunning)
            {
                var data = _client.Receive();
                if (data.Header == Header.IMG)
                {
                    var imgSource = ImageSource.FromStream(() => new MemoryStream(data.Data));
                    _dispatcher.ExecuteOnMainThreadAsync(() => Data = imgSource);
                }
                else if (data.Header == Header.USED)
                {
                    _userDialogs.Alert("Somebody using that streaming", "Error");
                }
            }
        }

        private void ToggleIsRunning()
        {
            IsRunning = !IsRunning;
            if (IsRunning == true)
            {
                _client.Send(new Message { Header = Header.START });
                receiveThread = new Thread(new ThreadStart(Receive));
                receiveThread.Start();
            }
            else
            {
                _client.Send(new Message { Header = Header.STOP });
                Data = null;
            }
        }
    }
}
