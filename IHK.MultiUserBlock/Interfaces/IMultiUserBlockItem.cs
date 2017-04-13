using IHK.Common;
using System;
using System.Net.WebSockets;

namespace IHK.MultiUserBlock.Interfaces
{
    public interface IMultiUserBlockItem
    {
        bool Active { get; set; }
        Enum Command { get; set; }
        WebSocket Socket { get; set; }
        DateTime UpdateTime { get; set; }
        bool Init { get; set; }
        string Description { get; set; }
        string SocketId { get; set; }
        EntityType EntityType { get; set; }
        int UserId { get; set; }
        int EntityId { get; set; }
        int Position { get; set; }
    }
}
