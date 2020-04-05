using System;
using System.Collections.Generic;
using System.Text;

namespace Sharing.Shared
{
    [Serializable]
    public class Message
    {
        public Header Header { get; set; }
        public byte[] Data { get; set; }

    }
}
