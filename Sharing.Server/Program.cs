using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Sharing.Server
{
    class Program
    {
        static UDPServer server;
        static bool isRunning;
        static Thread recordingThread;
        static void Main(string[] args)
        {
            server = new UDPServer();
            server.InitilizeServer(9050);
            var receiveThread = new Thread(new ThreadStart(Receive));
            receiveThread.Start();
        }

        public static void Receive()
        {
            while(true)
            {
                var msg = server.Receive();
                if (msg != null)
                {
                    if (msg.Header == Shared.Header.START)
                    {
                        if (isRunning)
                        {
                            var sendMsg = new Shared.Message
                            {
                                Data = Encoding.UTF8.GetBytes($"Server already sharing"),
                                Header = Shared.Header.USED
                            };
                            server.Send(sendMsg);
                        }
                        else
                        {
                            isRunning = true;
                            recordingThread = new Thread(new ThreadStart(Recording));
                            recordingThread.Start();
                        }
                    }
                    else if (msg.Header == Shared.Header.STOP)
                    {
                        isRunning = false;                    
                    }
                }
            }
        }

        public static void Recording()
        {
            while(isRunning)
            {
                var width = Screen.PrimaryScreen.Bounds.Width;
                var height = Screen.PrimaryScreen.Bounds.Height;
                Bitmap bitmap = new Bitmap(width , height);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                Bitmap resized = new Bitmap(bitmap, new Size(width / 4, height / 4));
                MemoryStream ms = new MemoryStream();
                resized.Save(ms, ImageFormat.Png);
                var bytes = ms.GetBuffer();
                var message = new Shared.Message()
                {
                    Data = bytes,
                    Header = Shared.Header.IMG
                };
                server.Send(message);
            }
        }

    }
}
