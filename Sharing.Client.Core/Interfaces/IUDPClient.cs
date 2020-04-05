using Sharing.Shared;
using Xamarin.Forms;

namespace Sharing.Client.Core.Interfaces
{
    public interface IUDPClient
    {
        void Initialize(string address, int port);

        Message Receive();

        void Send(Message message);
    }
}
