using Sharing.Shared;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sharing.Server
{
    public class UDPServer
    {
        private EndPoint remote;
        private Socket socket;
        private IFormatter formatter;

        public UDPServer()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            formatter = new BinaryFormatter();
        }
        public void InitilizeServer(int port)
        {
            var ipAdress = new IPEndPoint(IPAddress.Any, port);
            
            remote = new IPEndPoint(IPAddress.Any, 0);
            socket.Bind(ipAdress);
        }
        public Message Receive()
        {
            try
            {
                var data = new byte[1024];
                var recv = socket.ReceiveFrom(data, ref remote);
                var ms = new MemoryStream(data);
                return (Message)formatter.Deserialize(ms);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Failed to receive message {e.Message}");
                return null;
            }
        }

        public void Send(Message message)
        {
            byte[] bytes = null;
            try
            {
                var ms = new MemoryStream();
                formatter.Serialize(ms, message);
                bytes = ms.ToArray();
                socket.SendTo(bytes, remote);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to send {bytes.Length } bytes {e.Message}");
            }
        }
    }
}
