using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace IHK.Common
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

    public interface IMultiUserBlockViewModel
    {
        Enum Command { get; set; }
        string SocketId { get; set; }
        EntityType EntityType { get; set; }
        int UserId { get; set; }
        int EntityId { get; set; }
        int Position { get; set; }
        string Name { get; set; }
        string Vorname { get; set; }
        string Description { get; set; }
        string Telefon { get; set; }
        List<IMultiUserBlockViewModel> Waits { get; set; }
    }

    public interface IMultiUserBlockWebService
    {
        Task<IMultiUserBlockViewModel> Request(EntityType entityType, int entityId, int userId, string description);
        Task<IMultiUserBlockViewModel> Map(IMultiUserBlockItem block);
    }
}
