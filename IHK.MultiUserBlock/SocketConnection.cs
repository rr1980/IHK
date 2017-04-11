using IHK.Common;
using System.Net.WebSockets;

namespace IHK.MultiUserBlock
{
    public class SocketConnection
    {
        public WebSocket Socket { get; set; }
        public string SocketId { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public EntityType EntityType { get; set; }
        //public int Position { get; set; }
    }

}
