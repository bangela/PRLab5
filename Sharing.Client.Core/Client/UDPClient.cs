using Sharing.Client.Core.Interfaces;
using Sharing.Shared;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sharing.Client.Core.Client
{
    public class UDPClient : IUDPClient
    {
        private Socket socket;
        private EndPoint remote;
        private IPEndPoint ipAdress;
        private IFormatter formatter;

        public UDPClient()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            remote = new IPEndPoint(IPAddress.Any, 0);
            formatter = new BinaryFormatter();
        }
        public void Initialize(string address, int port)
        {
            IPAddress broadcast = IPAddress.Parse(address);
            ipAdress = new IPEndPoint(broadcast, port);
        }

        public void Send(Message message)
        {
            try
            {
                var ms = new MemoryStream();
                formatter.Serialize(ms, message);
                socket.SendTo(ms.GetBuffer(), ipAdress);
            }
            catch (Exception e)
            {
                Console.WriteLine("sendMessage exception: " + e.Message);
            }
        }
        public Message Receive()
        {
            var data = new byte[100000];
            var recv = socket.ReceiveFrom(data, ref remote);
            var ms = new MemoryStream(data);

            return (Message)formatter.Deserialize(ms);
        }
    }
}
